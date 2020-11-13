using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltController : MonoBehaviour
{
    public float tilt_speed, rotate_speed;

    void Update()
    {
        transform.Rotate(0f, 0f, tilt_speed * -Input.GetAxis("TiltZ"));
        transform.Rotate(tilt_speed * Input.GetAxis("TiltX"), 0f, 0f);
        transform.Rotate(0f, rotate_speed * Input.GetAxis("Rotate"), 0f);
        Debug.Log(Input.GetAxis("Rotate"));
    }
}
