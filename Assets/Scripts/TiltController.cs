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

    void FixedUpdate()
    {
        // Fix weird platform behaviour
        phy.angularVelocity = Vector3.zero;

        // Platform rotation
        Quaternion rot = Quaternion.AngleAxis(tilt_speed * -Input.GetAxis("Horizontal"), Vector3.forward);
        rot *= Quaternion.AngleAxis(tilt_speed * Input.GetAxis("Vertical"), Vector3.right);
        rot *= Quaternion.Euler(0f, rotate_speed * Input.GetAxis("Rotate"), 0f);
        phy.MoveRotation(rot * transform.rotation);
        phy.angularVelocity = Vector3.zero;

        sync.rotation = phy.rotation;
        sync.position = phy.position;
    }
}