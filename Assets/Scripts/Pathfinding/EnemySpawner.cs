using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform RemoteLevel;
    public Transform StaticLevel;
    public GameObject EnemyAI;
    public GameObject RemoteEnemy;
    // public Transform Position;
    public void Spawn(Vector3 position, Quaternion rotation) {
        var newEnemyAI = Instantiate(EnemyAI, position, rotation, this.RemoteLevel);
        var newRemoteEnemy = Instantiate(RemoteEnemy, position, rotation, this.StaticLevel);
        AddNewRemoteEnemyToNewAI(newEnemyAI, newRemoteEnemy);
    }

    private void AddNewRemoteEnemyToNewAI(GameObject newEnemyAI, GameObject newRemoteEnemy)
    {
        EnemyAI enemyAiInstance = newEnemyAI.GetComponent<EnemyAI>();
        enemyAiInstance.RemoteEnemy = newRemoteEnemy;

        SyncRemoteObject syncRemoteObject = newEnemyAI.GetComponent<SyncRemoteObject>();
        syncRemoteObject.remote_object = newRemoteEnemy.transform;
    }
}
