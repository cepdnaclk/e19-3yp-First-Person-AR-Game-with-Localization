import cv2
import numpy as np

img = cv2.imread("test")
cv2.imshow("test",img)
imgResult = img.copy()

def get_contours(img):
    contours,hierarchy = cv2.findContours(img, cv2.RETR_EXTERNAL,cv2.CHAIN_APPROX_NONE)
    x,y,w,h = 0,0,0,0
    #print(contours)
    for cnt in contours:
        area = cv2.contourArea(cnt)
        print(area)
        if area<2500:
            cv2.drawContours(imgResult, cnt, -1, (25, 255, 0), 5)
            peri = cv2.arcLength(cnt,True)
            approx = cv2.approxPolyDP(cnt,0.02*peri,True)
            x, y, w, h = cv2.boundingRect(approx)
    return x+w//2,y


def findColor():
    imgHSV = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)
    lower_red = np.array([0, 120, 70])
    upper_red = np.array([10, 255, 255])
    mask = cv2.inRange(imgHSV, lower_red, upper_red)
    x, y = get_contours(mask)

findColor()


#hsv min and max values
lower_red = np.array([0, 120, 70])
upper_red = np.array([10, 255, 255])


cv2.imshow("tes",imgResult)

cv2.waitKey(0)