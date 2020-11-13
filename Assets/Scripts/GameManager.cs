using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform ball;
    private Vector3 initial_position;
    private Rigidbody ball_phy;

    private void Start()
    {
        initial_position = ball.position;
        ball_phy = ball.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (ball.transform.position.y < -20f)
        {
            ball_phy.velocity = Vector3.zero;
            ball.position = initial_position;
        }
    }
}
