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


        // TODO Jonas fixx das mal <3
        //enemyAiInstance.RemoteEnemy = newRemoteEnemy;

        SyncRemoteObject syncRemoteObject = newEnemyAI.GetComponent<SyncRemoteObject>();
        syncRemoteObject.remote_object = newRemoteEnemy.transform;
    }

    public void AddAiToRemoteEnemy(GameObject remoteEnemy) {
        Destroy(remoteEnemy.GetComponent<Rigidbody>());
        var newEnemyAI = Instantiate(EnemyAI, 
                                     this.StaticLevel.worldToLocalMatrix * remoteEnemy.transform.position,
                                     Quaternion.identity,
                                     this.RemoteLevel);
        this.AddNewRemoteEnemyToNewAI(newEnemyAI, remoteEnemy);
    }
}
