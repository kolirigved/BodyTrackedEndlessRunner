import cv2
import mediapipe as mp

import socket

def send_data(data):

    host, port = "127.0.0.1", 25001

    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    


    try:
        sock.connect((host, port))
        sock.sendall(data.encode("utf-8"))
        # print(data)

    except Exception as e:
        print(e)

    finally:
        sock.close()

# Initialize MediaPipe Pose module
mp_pose = mp.solutions.pose
pose = mp_pose.Pose(min_detection_confidence=0.5, min_tracking_confidence=0.5)

# Initialize OpenCV to capture video
cap = cv2.VideoCapture(0)

# Variables for jump detection
jump_threshold = 0.06  # Adjust threshold for jump sensitivity
last_waist_y = None
is_jumping = False

while cap.isOpened():
    ret, frame = cap.read()
    if not ret:
        break

    # Convert the frame to RGB
    rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

    # Process the frame to detect pose landmarks
    results = pose.process(rgb_frame)

    # Convert back to BGR for displaying
    frame = cv2.cvtColor(rgb_frame, cv2.COLOR_RGB2BGR)
    frame = cv2.flip(frame, 1)

    # Draw pose landmarks on the frame
    if results.pose_landmarks:
        for landmark in results.pose_landmarks.landmark:
            landmark.x = 1- landmark.x
        mp.solutions.drawing_utils.draw_landmarks(frame, results.pose_landmarks, mp_pose.POSE_CONNECTIONS)

        # Continuously get the coordinates of the shoulder
        left_shoulder = results.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_SHOULDER]
        right_shoulder = results.pose_landmarks.landmark[mp_pose.PoseLandmark.RIGHT_SHOULDER]

        left_hip = results.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_HIP]
        right_hip = results.pose_landmarks.landmark[mp_pose.PoseLandmark.RIGHT_HIP]
        waist_y = (left_hip.y + right_hip.y) / 2
        waist_y = 1- waist_y

        centre_locx = (left_shoulder.x + right_shoulder.x) / 2
        centre_locy = (left_shoulder.y + right_shoulder.y) / 2
        centre_locy = 1- centre_locy
        # round off centre_loc to 4 decimal
        centre_locx = round(centre_locx, 4)
        centre_locy = round(centre_locy, 4)
        waist_y = round(waist_y, 4)
        
        # Print the coordinates of the shoulders
        

        send_data(str(centre_locx) + " " + str(centre_locy)+ " " + str(waist_y))
    
    # Display the frame
    cv2.imshow("Jump Checker", frame)

    # Exit on 'q' key press
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# Release resources and close windows
cap.release()
cv2.destroyAllWindows()
