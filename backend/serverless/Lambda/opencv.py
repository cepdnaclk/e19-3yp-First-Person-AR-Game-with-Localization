import cv2
import boto3
import numpy as np
from io import BytesIO
import base64
import json


client = boto3.client('lambda')
def zoom(img, zoom_factor=2):
    return cv2.resize(img, None, fx=zoom_factor, fy=zoom_factor)


def imread_from_base64(base64_string):
    image_binary_data = base64.b64decode(base64_string)
    bytes_io = BytesIO(image_binary_data)
    # Use cv2.imdecode to read the image from BytesIO
    image = cv2.imdecode(np.frombuffer(bytes_io.read(), np.uint8), cv2.IMREAD_COLOR)

    return image






qcd = cv2.QRCodeDetector()

def get_contours(img):
    contours,hierarchy = cv2.findContours(img, cv2.RETR_EXTERNAL,cv2.CHAIN_APPROX_NONE)
    X,Y,W,H = 0,0,0,0
    cur_area = 0
    #print(contours)
    for cnt in contours:
        area = cv2.contourArea(cnt)
        
        if area>2:
            # cv2.drawContours(imgResult, cnt, -1, (0, 255, 0), 6)
            peri = cv2.arcLength(cnt,True)
            approx = cv2.approxPolyDP(cnt,0.02*peri,True)
            x, y, w, h = cv2.boundingRect(approx)

            #LOGIC to remove outliers
            if area>cur_area:
                cur_area = area
                X, Y, W, H = x, y, w, h
    return X+W//2,Y


def findColor(img, points, decoded_info):
    imgHSV = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)
    lower_red = np.array([0, 50, 50])
    upper_red = np.array([10, 255, 255])
    mask = cv2.inRange(imgHSV, lower_red, upper_red)
    x, y = get_contours(mask)
   

    for qrs_id in range(len(points)):
        width = abs(points[qrs_id][1][0] - points[qrs_id][0][0])
        if points[qrs_id][0][0] <=x and points[qrs_id][1][0]>=x:
            if points[qrs_id][0][1] <=y and points[qrs_id][2][1]>=y:
                return decoded_info[qrs_id]
    return False

def lambda_handler(event, context):
    body_dict = json.loads(event['body'])
    encoded_str = body_dict['img']
    shooter = body_dict['email']
    img_un = imread_from_base64(encoded_str)
    row, col = img_un.shape[0], img_un.shape[1]

    #remove corner pixels
    img_c = img_un[(1 * row) // 8:(5 * row) // 8, (3 * col) // 16:(5 * col) // 8]

    #stretch pixels
    img = zoom(img_c, 3)
    retval, decoded_info, points, straight_qrcode = qcd.detectAndDecodeMulti(img)
    if retval:
        result = findColor(img, points, decoded_info)
        if result and result != "":
            socketmsg = {"hit": result, "shooter": shooter}
            client.invoke( FunctionName='opencvsocket', InvocationType='Event', Payload=json.dumps(socketmsg))
            
            return {"statusCode": 200, "body": result}
            
        else:
            return {"statusCode":404, "body": "Not a hit"}
             #return {"hit": "false"}
    else:
        return {"statusCode":404, "body": "Not a hit"}
        

