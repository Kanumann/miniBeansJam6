using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Transform ball;
    private Vector3 initial_position;
    private Rigidbody ball_phy;
    private EnemySpawner EnemySpawnerInstance;

    public Transform staticLevelHelper;

    private void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Player").transform;
        initial_position = ball.position;
        ball_phy = ball.GetComponent<Rigidbody>();
        this.EnemySpawnerInstance = GetComponent<EnemySpawner>();
    }

    public GameObject RegisterNewEnemy(GameObject newRemoteEnemy)
    {
        return EnemySpawnerInstance.AddAiToRemoteEnemy(newRemoteEnemy);
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
