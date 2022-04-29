using UnityEngine;
using System;

internal enum DriveType
{
    frontWheelDrive,
    backWheelDrive,
    allWheelDrive
}

public class Locomotion : MonoBehaviour
{

    // drive type
    [SerializeField] private DriveType driveType = DriveType.backWheelDrive;

    // wheel mesh variables
    Quaternion quaternion;
    Vector3 position;

    // maximum steering angle
    [SerializeField] private float _maxSteeringAngle = 30.0f;

    // the collection of wheels
    [SerializeField] private WheelCollider[] _wheelColliders;

    // torque
    [SerializeField] private float torque = 250.0f;


    void Update() {
        float acceleration = Input.GetAxis("Vertical");
        float steering = Input.GetAxis("Horizontal");
        Move(acceleration, steering);
    }

    private void Move(float acceleration, float steering) {
        // ensure the values are clamped
        acceleration = Math.Clamp(acceleration, -1f, 1f);
        steering = Mathf.Clamp(steering, -1f, 1f) * _maxSteeringAngle;

        // calculate the thrust torque
        float thrustTorque = acceleration * torque;

        // apply thrust to each and every wheel
        for (int i = 0; i < _wheelColliders.Length; i++) {
            _wheelColliders[i].motorTorque = thrustTorque;

            // get the position and rotation of the wheel collider
            _wheelColliders[i].GetWorldPose(out position, out quaternion);
            // reposition the game object with the mesh of the wheel
            _wheelColliders[i].transform.GetChild(0).transform.position = position;
            // apply the rotation to the game object
            _wheelColliders[i].transform.GetChild(0).transform.rotation = quaternion;

            if (i < 2)
            {
                _wheelColliders[i].steerAngle = steering;
            }
        }
    }

}
