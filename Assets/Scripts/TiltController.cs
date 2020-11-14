using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltController : MonoBehaviour
{
    public float tilt_speed, rotate_speed;
    public Transform sync;
    private Rigidbody phy;

    private void Awake()
    {
        phy = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Platform rotation
        Quaternion rot = transform.rotation * Quaternion.Euler(0f, 0f, tilt_speed * -Input.GetAxis("TiltZ"));
        rot *= Quaternion.Euler(tilt_speed * Input.GetAxis("TiltX"), 0f, 0f);
        rot *= Quaternion.Euler(0f, rotate_speed * Input.GetAxis("Rotate"), 0f);
        phy.MoveRotation(rot);

        sync.rotation = phy.rotation;
        sync.position = phy.position;
    }
}