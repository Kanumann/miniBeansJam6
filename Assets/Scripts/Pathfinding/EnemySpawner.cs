using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform RemoteLevel;
    public Transform StaticLevel;
    public GameObject EnemyAI;
    private EnemyController enemyController;

    void Start() {
        enemyController = gameObject.GetComponent<EnemyController>();
    }

    public GameObject AddAiToRemoteEnemy(GameObject newRemoteEnemy)
    {
        var newEnemyAI = Instantiate(EnemyAI,
                                     this.RemoteLevel.TransformPoint(newRemoteEnemy.transform.localPosition),
                                     Quaternion.identity,
                                     this.RemoteLevel);
        newEnemyAI.name = "EnemyPathFinder";

        // sync with remote level
        SyncRemoteObject syncRemoteObject = newEnemyAI.GetComponent<SyncRemoteObject>();
        syncRemoteObject.remote_object = newRemoteEnemy.transform;

        // add to enemy controller
        enemyController.addEnemy(newEnemyAI);

        return newEnemyAI;
    }
}
