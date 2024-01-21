import cv2
import numpy as np

img = cv2.imread('236.png')

qcd = cv2.QRCodeDetector()

#retreval if detected,
#detected_info = decoded strings
#points = cordinates
retval, decoded_info, points, straight_qrcode = qcd.detectAndDecodeMulti(img)
print(decoded_info)

