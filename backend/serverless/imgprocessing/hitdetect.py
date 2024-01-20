import cv2
import numpy as np

img = cv2.imread("test")
cv2.imshow("test",img)

#hsv min and max values
lower_red = np.array([0, 120, 70]) 
upper_red = np.array([10, 255, 255]) 


cv2.waitKey(0)