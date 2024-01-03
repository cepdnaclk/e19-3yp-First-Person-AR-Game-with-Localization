import cv2

# Load pre-trained HOG detector for people
hog = cv2.HOGDescriptor()
hog.setSVMDetector(cv2.HOGDescriptor_getDefaultPeopleDetector())

# Open video capture
cap = cv2.VideoCapture('http://100.92.89.144:4747/video')  # Replace 'your_video.mp4' with your video file
frame_count = 0

while True:
    # Read each frame
    ret, frame = cap.read()
    if not ret:
        break
    frame_count += 1
    if frame_count % 32 == 0:  # Process every other frame (adjust as needed)
        continue

    # Resize frame for faster processing if needed
    frame = cv2.resize(frame, (640, 480))

    # Detect people in the frame
    (rects, _) = hog.detectMultiScale(frame, winStride=(4, 4), padding=(8, 8), scale=1.05)

    # Draw rectangles around detected people
    for (x, y, w, h) in rects:
        cv2.rectangle(frame, (x, y), (x + w, y + h), (0, 255, 0), 2)

    # Display the resulting frame
    cv2.imshow('Detected People', frame)

    # Break the loop when 'q' is pressed
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# Release video capture and close windows
cap.release()
cv2.destroyAllWindows()
