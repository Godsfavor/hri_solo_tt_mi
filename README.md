# BlueROV Unity Control

This is a Unity project that lets you control a BlueROV2 underwater robot using either Unity's keyboard controls or ROS2 teleop. It also streams the camera feed to ROS.

## What does it do?

You can control the BlueROV in Unity using your keyboard. The robot can move in all directions and rotate however you want. When Unity has focus, you control it. When you switch to your terminal, ROS teleop takes over. No conflicts, it just works.

## The Scripts

**BlueRovVeloControl.cs** - Makes the robot move based on velocity commands. Handles the actual movement and rotation.

**BlueRovCmdVeloRosSub.cs** - Listens to the `/bluerov1/cmd_vel` ROS topic and tells the robot to move.

**KeyboardCmdVelPublisher.cs** - Sends keyboard inputs to ROS. Only works when Unity window is focused, so it doesn't fight with ROS teleop.

**CameraCapturer.cs** - Grabs frames from the Unity camera.

**CameraRosPublisher.cs** - Publishes those camera frames to ROS at 10 fps.

## Controls

When the Unity window is focused, use these keys:

**Moving around:**
- W = left
- S = right  
- A = back
- D = forward
- Q = up
- E = down

**Rotating:**
- R = pitch nose down
- F = pitch nose up
- T = roll left side up
- G = roll right side up
- Z = turn left
- C = turn right

## Setup

1. **Start the ROS endpoint:**
```bash
ros2 run ros_tcp_endpoint default_server_endpoint
```

2. **Start Unity and press Play**

3. **Choose your control method:**
   - Click Unity window → use keyboard controls above
   - Or run ROS teleop in terminal:
   ```bash
   ros2 run teleop_twist_keyboard teleop_twist_keyboard --ros-args -r /cmd_vel:=/bluerov1/cmd_vel
   ```

That's it. Switch between Unity and terminal by clicking the window you want to use.

## How to add the scripts in Unity

Attach these to the BlueROV2 GameObject:
- BlueRovVeloControl.cs
- BlueRovCmdVeloRosSub.cs  
- KeyboardCmdVelPublisher.cs

Attach these to the Camera GameObject (child of BlueROV2):
- CameraCapturer.cs
- CameraRosPublisher.cs

Make sure the BlueROV2 has a Rigidbody component.

## Common issues

**Robot doesn't move** - Check that ROS endpoint is running and Unity's ROS settings have the right IP (probably 127.0.0.1).

**Rotations don't work** - Check BlueRovVeloControl.cs has avx, avy, and avz variables.

**Both Unity and ROS trying to control at once** - Only one window should have focus. The KeyboardCmdVelPublisher only publishes when Unity is focused.

## ROS Topics

- `/bluerov1/cmd_vel` - velocity commands
- `/bluerov1/camera` - camera feed

Check topics with:
```bash
ros2 topic list
ros2 topic echo /bluerov1/cmd_vel
```

## Viewing the Camera

### Using RViz2
```bash
rviz2

### Using rqt
```bash
rqt_image_view


### Terminal 1 – ROS TCP Endpoint (for communication bridge)
ros2 run ros_tcp_endpoint default_server_endpoint

### Terminal 2 – Keyboard Teleoperation
ros2 run teleop_twist_keyboard teleop_twist_keyboard \
  --ros-args -r /cmd_vel:=/bluerov1/cmd_vel

### Terminal 3 – View the camera
rqt_image_view
# → select /bluerov1/camera
   
