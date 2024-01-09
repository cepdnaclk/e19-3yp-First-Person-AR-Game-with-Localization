#Capture the video from Droid Cam and process the image

import cv2
import keyboard
import numpy as np
import imutils

# Set the DroidCam IP address and port
droidcam_ip = "10.231.89.42"
droidcam_port = "4747"

droidcam_url = f"http://{droidcam_ip}:{droidcam_port}/video"

cap = cv2.VideoCapture(droidcam_url)

while True:
    ret, frame = cap.read()
    cv2.imshow("DroidCam Feed", frame)

    if keyboard.is_pressed('s'):
        cv2.imwrite("img.jpg", frame)
        print("Image captured!")
        break  # exit the loop after capturing the image

    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

cap.release()

protopath = "MobileNetSSD_deploy.prototxt"
modelpath = "MobileNetSSD_deploy.caffemodel"
detector = cv2.dnn.readNetFromCaffe(prototxt=protopath, caffeModel=modelpath)

CLASSES = ["background", "aeroplane", "bicycle", "bird", "boat",
           "bottle", "bus", "car", "cat", "chair", "cow", "diningtable",
           "dog", "horse", "motorbike", "person", "pottedplant", "sheep",
           "sofa", "train", "tvmonitor"]

def main():
    image = cv2.imread('img.jpg')  # Image from the unity app
    image = imutils.resize(image, width=600)

    (H, W) = image.shape[:2]

    blob = cv2.dnn.blobFromImage(image, 0.007843, (W, H), 127.5)

    detector.setInput(blob)
    person_detections = detector.forward()  # Take all the detection results

    for i in np.arange(0, person_detections.shape[2]):
        confidence = person_detections[0, 0, i, 2]
        if confidence > 0.6:
            idx = int(person_detections[0, 0, i, 1])

            if CLASSES[idx] != "person":
                continue

            person_box = person_detections[0, 0, i, 3:7] * np.array([W, H, W, H])
            (startX, startY, endX, endY) = person_box.astype("int")
            print("Person Number {}".format(i + 1), startX, startY, endX, endY)
            cv2.rectangle(image, (startX, startY), (endX, endY), (0, 0, 255), 2)

    cv2.imshow("Results", image)

    # Wait for a key press (0 means wait indefinitely)
    key = cv2.waitKey(0)

    # If 'q' is pressed, close the window
    if key == ord('q'):
        cv2.destroyAllWindows()

main()

