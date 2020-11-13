using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Transform AiTransform;
    Transform EnemyTransform;
    void Start()
    {
        AiTransform = GameObject.Find("AI").GetComponent<Transform>();
        EnemyTransform = gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        // Bind position of AI to position of enemy
        var enemyX = EnemyTransform.position.x;
        var enemyZ = EnemyTransform.position.z;
        AiTransform.position = new Vector3(enemyX, 0, enemyZ);
    }
}
