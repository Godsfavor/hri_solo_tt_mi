using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;

public class KeyboardCmdVelPublisher : MonoBehaviour
{
    public string topic = "/bluerov1/cmd_vel";
    public float forwardSpeed = 0.5f;      // X-axis (forward/backward)
    public float strafeSpeed = 0.5f;       // Y-axis (left/right strafe)
    public float verticalSpeed = 0.5f;     // Z-axis (up/down)
    public float rollSpeed = 0.5f;         // Roll (rotation around X-axis)
    public float pitchSpeed = 0.5f;        // Pitch (rotation around Y-axis)
    public float yawSpeed = 0.5f;          // Yaw (rotation around Z-axis)
    
    [Header("Control Settings")]
    [Tooltip("Only publish commands when Unity window has focus")]
    public bool onlyWhenFocused = true;

    private ROSConnection ros;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TwistMsg>(topic);
    }

    void Update()
    {
        // Only process input if Unity window has focus (or if onlyWhenFocused is disabled)
        if (onlyWhenFocused && !Application.isFocused)
        {
            return; // Don't publish anything when Unity doesn't have focus
        }

        var twist = new TwistMsg();
        bool anyKeyPressed = false;

        // ===== TRANSLATIONS =====
        
        // W/S keys - Left/Right translation
        if (Input.GetKey(KeyCode.W))
        {
            twist.linear.x = forwardSpeed;      // Left translation
            anyKeyPressed = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            twist.linear.x = -forwardSpeed;     // Right translation
            anyKeyPressed = true;
        }

        // A/D keys - Back/Forward translation
        if (Input.GetKey(KeyCode.A))
        {
            twist.linear.y = strafeSpeed;       // Back translation
            anyKeyPressed = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            twist.linear.y = -strafeSpeed;      // Forward translation
            anyKeyPressed = true;
        }

        // Q/E keys - Up/Down translation
        if (Input.GetKey(KeyCode.Q))
        {
            twist.linear.z = verticalSpeed;     // Upward translation
            anyKeyPressed = true;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            twist.linear.z = -verticalSpeed;    // Downward translation
            anyKeyPressed = true;
        }

        // ===== ROTATIONS =====
        
        // R/F keys - Pitch (nose up/down)
        if (Input.GetKey(KeyCode.R))
        {
            twist.angular.x = rollSpeed;        // Downward pitch (nose of BlueRov faces down)
            anyKeyPressed = true;
        }
        else if (Input.GetKey(KeyCode.F))
        {
            twist.angular.x = -rollSpeed;       // Upward pitch (nose of BlueRov faces up)
            anyKeyPressed = true;
        }

        // T/G keys - Roll (side tilt)
        if (Input.GetKey(KeyCode.T))
        {
            twist.angular.y = pitchSpeed;       // Roll (left side of BlueRov2 rises/faces upward)
            anyKeyPressed = true;
        }
        else if (Input.GetKey(KeyCode.G))
        {
            twist.angular.y = -pitchSpeed;      // Roll (right side of BlueRov2 rises/faces upward)
            anyKeyPressed = true;
        }

        // Z/C keys - Yaw (turn left/right)
        if (Input.GetKey(KeyCode.Z))
        {
            twist.angular.z = yawSpeed;         // Yaw (front of BlueRov2 turns left)
            anyKeyPressed = true;
        }
        else if (Input.GetKey(KeyCode.C))
        {
            twist.angular.z = -yawSpeed;        // Yaw (front of BlueRov2 turns right)
            anyKeyPressed = true;
        }

        // Only publish if Unity has focus and a key is pressed
        // This prevents sending zero commands when switching to terminal
        if (anyKeyPressed)
        {
            ros.Publish(topic, twist);
            Debug.Log($"Unity Publishing: linear({twist.linear.x:F2}, {twist.linear.y:F2}, {twist.linear.z:F2}) angular({twist.angular.x:F2}, {twist.angular.y:F2}, {twist.angular.z:F2})");
        }
    }
}