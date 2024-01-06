import cv2
import numpy as np
from mpu6050 import mpu6050

# Initialize the MPU6050 sensor
sensor = mpu6050(0x68)

# Load the video
cap = cv2.VideoCapture('video.mp4')

# Initialize the crosshair position
x, y = 0, 0

while(cap.isOpened()):
    # Get accelerometer and gyro data
    accel_data = sensor.get_accel_data()
    gyro_data = sensor.get_gyro_data()

    # Update crosshair position based on sensor data
    x += gyro_data['x']
    y += gyro_data['y']

    # Read the next frame from the video
    ret, frame = cap.read()

    if ret:
        # Draw the crosshair on the frame
        frame = cv2.circle(frame, (int(x), int(y)), radius=5, color=(0, 0, 255), thickness=-1)

        # Display the resulting frame
        cv2.imshow('Video', frame)

        # Break the loop on 'q' key press
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break
    else:
        break

# Release the video capture and close windows
cap.release()
cv2.destroyAllWindows()