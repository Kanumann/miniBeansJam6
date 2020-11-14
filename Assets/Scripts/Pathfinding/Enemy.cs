using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Transform AiTransform;
    public Transform EnemyTransform;
    public Rigidbody EnemyRigidbody;
    void Start()
    {
        AiTransform = GameObject.Find("AI").GetComponent<Transform>();
    }

    void Update()
    {
        EnemyTransform.LookAt(AiTransform);
        EnemyRigidbody.AddRelativeForce(new Vector3(-0.1f, 0, 0));        
    }
}
