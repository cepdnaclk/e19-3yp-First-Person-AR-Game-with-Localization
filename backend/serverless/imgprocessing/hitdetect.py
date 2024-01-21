import cv2
import numpy as np

img = cv2.imread("qr.jpg")
cv2.imshow("test",img)
imgResult = img.copy()

qcd = cv2.QRCodeDetector()
retval, decoded_info, points, straight_qrcode = qcd.detectAndDecodeMulti(img)
#print(points)
#print(decoded_info)
def get_contours(img):
    contours,hierarchy = cv2.findContours(img, cv2.RETR_EXTERNAL,cv2.CHAIN_APPROX_NONE)
    x,y,w,h = 0,0,0,0
    #print(contours)
    for cnt in contours:
        area = cv2.contourArea(cnt)
        #print(area)
        if area>2:
            cv2.drawContours(imgResult, cnt, -1, (0, 255, 0), 6)
            peri = cv2.arcLength(cnt,True)
            approx = cv2.approxPolyDP(cnt,0.02*peri,True)
            x, y, w, h = cv2.boundingRect(approx)
    return x+w//2,y


def findColor():
    imgHSV = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)
    lower_red = np.array([0, 50, 50])
    upper_red = np.array([10, 255, 255])
    mask = cv2.inRange(imgHSV, lower_red, upper_red)
    x, y = get_contours(mask)
    #print(points)
    for qrs_id in range(len(points)):
        if points[qrs_id][0][0] <=x and points[qrs_id][1][0]>=x:
            if points[qrs_id][0][1] <=y and points[qrs_id][2][1]>=y:
                print("hit", decoded_info[qrs_id])

findColor()


#hsv min and max values
lower_red = np.array([0, 120, 70])
upper_red = np.array([10, 255, 255])


cv2.imshow("tes",imgResult)

cv2.waitKey(0)