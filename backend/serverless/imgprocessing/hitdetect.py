import cv2
import numpy as np

img = cv2.imread("test")
cv2.imshow("test",img)
imgResult = img.copy()

def get_contours():
    contours,hierarchy = cv2.findContours(img, cv2.RETR_EXTERNAL,cv2.CHAIN_APPROX_NONE)
    x,y,w,h = 0,0,0,0
    for cnt in contours:
        area = cv2.contourArea(cnt)
        if area<2500:
            cv2.drawContours(imgResult, cnt, -1, (255, 0, 0), 3)
            peri = cv2.arcLength(cnt,True)
            approx = cv2.approxPolyDP(cnt,0.02*peri,True)
            x, y, w, h = cv2.boundingRect(approx)
    return x+w//2,y

#hsv min and max values
lower_red = np.array([0, 120, 70])
upper_red = np.array([10, 255, 255])




cv2.waitKey(0)