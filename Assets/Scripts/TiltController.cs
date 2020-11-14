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
        Quaternion rot = transform.rotation * Quaternion.Euler(0f, 0f, tilt_speed * -Input.GetAxis("Horizontal")); // Z
        rot *= Quaternion.Euler(tilt_speed * Input.GetAxis("Vertical"), 0f, 0f); // X
        rot *= Quaternion.Euler(0f, rotate_speed * Input.GetAxis("Rotate"), 0f); // Y
        phy.MoveRotation(rot);

        sync.rotation = phy.rotation;
        sync.position = phy.position;
    }
}