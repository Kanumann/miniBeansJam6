using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteEnemyBehaviour : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject pathFinderAI;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        gameObject.name = "RemoteEnemy";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (pathFinderAI != null) return;
        Destroy(GetComponent<Rigidbody>());
        transform.parent = gameManager.staticLevelHelper;
        gameObject.layer = 10;
        pathFinderAI = gameManager.RegisterNewEnemy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("Enemy died!");
        Destroy(pathFinderAI);
    }
}
