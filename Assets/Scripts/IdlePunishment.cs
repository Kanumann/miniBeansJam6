using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePunishment : MonoBehaviour
{
    public float radius, time_limit, cooldown;
    private Transform ball;
    private Rigidbody ball_phy;
    private float time;

    private void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Player").transform;
        ball_phy = ball.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if ((transform.position - ball.position).magnitude > radius)
        {
            time = 0f;
            transform.position = ball.position;
        }
        else
        {
            time += Time.deltaTime;
            if (time > time_limit) OnPlayerIdle();
        }
    }

    public void OnEnable()
    {
        time = 0f;
    }

    public void Activate()
    {
        enabled = true;
    }

    public void OnPlayerIdle()
    {
        enabled = false;
        Invoke("Activate", cooldown);
        time = 0f;
    }
}
