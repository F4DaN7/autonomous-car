# Autonomous car
The project demonstrates autonomous vehicle control in a 3D Unity environment using the Unity ML‑Agents toolkit (Release 19). The agent learns to navigate a track, maintain stable lane positioning, and adapt its steering based on continuous sensor input. Through reinforcement learning, the vehicle gradually improves its driving behavior, responding to the curvature of the road and adjusting its trajectory to stay on course. As training progresses, the agent becomes increasingly proficient, demonstrating smoother control, quicker adaptation to dynamic conditions, and more reliable long‑term navigation.
## Requirements 
- Unity 2021+
- Unity ML-Agents Release 19
- Python 3.8+
- PyTorch 1.10+

## TrackTraining
The model was trained on a short test track and used forward‑facing sensor data to understand the shape and direction of the road.

<img width="362" height="210" alt="obraz" src="https://github.com/user-attachments/assets/70acaac5-65d4-4c93-9050-8d5a07ef0182" />


## Performance Summary 
The trained model was evaluated on a different track and successfully completed the entire route.

<img width="555" height="178" alt="obraz" src="https://github.com/user-attachments/assets/09d4cb1a-73c6-4446-9d3a-4a33c91cf544" />
