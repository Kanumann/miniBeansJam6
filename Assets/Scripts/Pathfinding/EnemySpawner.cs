using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform RemoteLevel;
    public Transform StaticLevel;
    public GameObject EnemyAI;

    public GameObject AddAiToRemoteEnemy(GameObject newRemoteEnemy)
    {
        var newEnemyAI = Instantiate(EnemyAI,
                                     this.RemoteLevel.TransformPoint(newRemoteEnemy.transform.localPosition),
                                     Quaternion.identity,
                                     this.RemoteLevel);
        newEnemyAI.name = "EnemyPathFinder";

        SyncRemoteObject syncRemoteObject = newEnemyAI.GetComponent<SyncRemoteObject>();
        syncRemoteObject.remote_object = newRemoteEnemy.transform;

        return newEnemyAI;
    }
}
