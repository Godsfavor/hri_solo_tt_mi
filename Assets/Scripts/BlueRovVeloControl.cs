using UnityEngine;
using System;

public class BlueRovVeloControl : MonoBehaviour {
    public float lvx = 0.0f; 
    public float lvy = 0.0f;
    public float lvz = 0.0f;
    public float avx = 0.0f;  // Roll
    public float avy = 0.0f;  // Pitch
    public float avz = 0.0f;  // Yaw
    public bool movementActive = false;
    public Rigidbody rb;
    
    void Start() {
        this.rb = GetComponent<Rigidbody>();
    }
    
    private void moveVelocityRigidbody() {
        // Translation: ROS to Unity coordinate mapping
        Vector3 movement = new Vector3(-lvx * Time.fixedDeltaTime, 
                                       -lvz * Time.fixedDeltaTime,  
                                       lvy * Time.fixedDeltaTime);
        transform.Translate(movement, Space.Self);
        
        // Rotation: Apply roll, pitch, and yaw
        // ROS uses radians, Unity uses degrees
        float rollDeg = avx * Time.fixedDeltaTime * Mathf.Rad2Deg;
        float pitchDeg = avy * Time.fixedDeltaTime * Mathf.Rad2Deg;
        float yawDeg = avz * Time.fixedDeltaTime * Mathf.Rad2Deg;
        
        // Apply rotations in local space
        transform.Rotate(rollDeg, yawDeg, pitchDeg, Space.Self);
    }
    
    public void moveVelocity(RosMessageTypes.Geometry.TwistMsg velocityMessage) {
        // Linear velocities
        this.lvx = (float)velocityMessage.linear.x;
        this.lvy = (float)velocityMessage.linear.y;
        this.lvz = (float)velocityMessage.linear.z;
        
        // Angular velocities
        this.avx = (float)velocityMessage.angular.x;  // Roll
        this.avy = (float)velocityMessage.angular.y;  // Pitch
        this.avz = (float)velocityMessage.angular.z;  // Yaw
        
        this.movementActive = true;
    }

    void FixedUpdate() {
        if (movementActive) {
            moveVelocityRigidbody();
            this.movementActive = false;
        }
    }
}