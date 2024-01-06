import cv2
import socket

UDP_IP = "127.0.0.1"
UDP_PORT = 5065

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

# Load the pre-trained body detection model from OpenCV
body_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + 'haarcascade_fullbody.xml')

# Open Camera
try:
    default = 0  # Try changing it to 1 if webcam not found
    capture = cv2.VideoCapture(default)
except:
    print("No Camera Source Found!")

while capture.isOpened():
    # Capture frames from the camera
    ret, frame = capture.read()

    # Convert the frame to grayscale for body detection
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

    # Detect bodies in the frame using the cascade classifier
    bodies = body_cascade.detectMultiScale(gray, scaleFactor=1.1, minNeighbors=5)

    # Draw rectangles around the detected bodies
    for (x, y, w, h) in bodies:
        cv2.rectangle(frame, (x, y), (x + w, y + h), (255, 0, 0), 2)
        cv2.putText(frame, 'Human', (x, y - 10), cv2.FONT_HERSHEY_SIMPLEX, 0.9, (36, 255, 12), 2)

        # Send a message when a body is detected
        sock.sendto("BODY_DETECTED".encode(), (UDP_IP, UDP_PORT))
        print("Body Detected!")

    # Show the frame with body detection
    cv2.imshow('Human Detection', frame)

    # Close the camera if 'q' is pressed
    if cv2.waitKey(1) == ord('q'):
        break

capture.release()
cv2.destroyAllWindows()
