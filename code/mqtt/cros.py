import cv2
import numpy as np
import paho.mqtt.client as mqtt
import json

# Initialize the center of the cross at the middle of the frame
center = np.array([300, 300])

# Initialize the size of the cross
cross_size = 20

# Open the video feed
cap = cv2.VideoCapture(0)

def on_message(client, userdata, msg):
    global center, cross_size
    data = json.loads(msg.payload.decode())  # decode JSON data

    # Update the center of the cross using the X and Y rotations
    center[1] = 200 - 5 * int(data['Roll: '])
    # x = int(data['Pitch: '])
    # if x > 0:
    #     center[0] -= 15 * x
    # else:
    #     center[0] -=25 * x

    # if center[0] > 600:
    #     center[0] = 600

    # if center[0] < 0:
    #     center[0] = 0

    # Update the size of the cross using the Z rotation
    # cross_size += int(data['Acceleration Z: '])

    print(center, cross_size)

# Create an MQTT client
client = mqtt.Client()

# Assign the callback function to the client
client.on_message = on_message

# Connect to the broker
client.connect("localhost", 1884, 60)

# Subscribe to the topic
client.subscribe("gyro/pub")

# Start the MQTT loop
client.loop_start()

while True:
    # Capture frame-by-frame
    ret, frame = cap.read()

    # Draw the cross at the center of the frame with the updated size
    cv2.drawMarker(frame, tuple(center), (0, 255, 0), markerType=cv2.MARKER_CROSS, markerSize=cross_size)

    # Display the resulting frame
    cv2.imshow('Video feed', frame)

    # Break the loop on 'q' key press
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# When everything done, release the capture and destroy the windows
cap.release()
cv2.destroyAllWindows()

# Stop the MQTT loop
client.loop_stop()