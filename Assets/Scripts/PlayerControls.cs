using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    Rigidbody rigBody;
    public float moveFactor = 1;
    void Start()
    {
        rigBody = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Minimal player controls
        float x = Input.GetAxis("Horizontal") * moveFactor;
        float z = Input.GetAxis("Vertical") * moveFactor;
        rigBody.AddForce(new Vector3(x, 0, z));
    }
}
