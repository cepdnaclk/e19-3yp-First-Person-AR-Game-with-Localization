import cv2
import numpy as np
from io import BytesIO
import base64
import json

def imread_from_base64(base64_string):
    image_binary_data = base64.b64decode(base64_string)
    bytes_io = BytesIO(image_binary_data)

    # Use cv2.imdecode to read the image from BytesIO
    image = cv2.imdecode(np.frombuffer(bytes_io.read(), np.uint8), cv2.IMREAD_COLOR)

    return image


def image_to_base64(image_path):
    with open(image_path, "rb") as image_file:
        image_binary_data = image_file.read()
        base64_encoded = base64.b64encode(image_binary_data)
        base64_string = base64_encoded.decode("utf-8")
        image_file.close()
    return base64_string


# img_encode = image_to_base64("qr5.jpg")
# img = imread_from_base64(img_encode)
#
# #img = cv2.imread(img_decode)
# cv2.imshow("test",img)
# imgResult = img.copy()

qcd = cv2.QRCodeDetector()
# retval, decoded_info, points, straight_qrcode = qcd.detectAndDecodeMulti(img)
# #print(points)
# print(decoded_info)
def get_contours(img):
    contours,hierarchy = cv2.findContours(img, cv2.RETR_EXTERNAL,cv2.CHAIN_APPROX_NONE)
    X,Y,W,H = 0,0,0,0
    cur_area = 0
    #print(contours)
    for cnt in contours:
        area = cv2.contourArea(cnt)
        print(area)
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
    # print(points)
    # print(x, y)

    for qrs_id in range(len(points)):
        if points[qrs_id][0][0] <=x and points[qrs_id][1][0]>=x:
            if points[qrs_id][0][1] <=y and points[qrs_id][2][1]>=y:
                return decoded_info[qrs_id]
    return False

def lambda_handler(event, context):
    #print(event)
    body_dict = json.loads(event['body'])
    #print(body_dict)
    
    encoded_str = body_dict['img']
    img = imread_from_base64(encoded_str)
    retval, decoded_info, points, straight_qrcode = qcd.detectAndDecodeMulti(img)
    result = findColor(img, points, decoded_info)
    if result:
         return {"statusCode": 200, "body": result}
    else:
         return {"statusCode":404, "body": "Not a hit"}

#body: json.dumps
# findColor()


#hsv min and max values
# lower_red = np.array([0, 120, 70])
# upper_red = np.array([10, 255, 255])


# cv2.imshow("tes",imgResult)

#cv2.waitKey(0)