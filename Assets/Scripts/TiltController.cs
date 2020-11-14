using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltController : MonoBehaviour
{
    public float tilt_speed, rotate_speed;
    private Rigidbody phy;

    private void Awake()
    {
        phy = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Platform rotation
        Quaternion rot = Quaternion.AngleAxis(tilt_speed * -Input.GetAxis("TiltZ"), Vector3.forward);
        rot *= Quaternion.AngleAxis(tilt_speed * Input.GetAxis("TiltX"), Vector3.right);
        rot *= Quaternion.Euler(0f, rotate_speed * Input.GetAxis("Rotate"), 0f);
        phy.MoveRotation(rot * transform.rotation);
        phy.angularVelocity = Vector3.zero;
    }
}