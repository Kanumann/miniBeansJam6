using System.Collections;
using Pathfinding;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // TODO(Jonas): Overwrite maxDistanceToHit depending on player 
    //            :and enemy size
    public GameObject RemotePlayer;
    public GameObject RemoteEnemy;
    public float maxDistanceToHit;
    public float inactiveTime;

    private Vector3 RemoteEnemyPosition { 
        get { return RemoteEnemy.transform.position; } 
    }
    private Vector3 RemotePlayerPosition {
         get { return RemotePlayer.transform.position; } 
    }
    private Rigidbody RemotePlayerRigidBody {
         get { return RemotePlayer.GetComponent<Rigidbody>(); } 
    }
    private Vector3 InitialAiPosition;

    private AIPath AiPathInstance; 
    private bool CanMove { 
        get { return AiPathInstance.canMove; }
        set { AiPathInstance.canMove = value; }
    }

    private float checkForRespawnInterval = 2;
    // TODO public schnittstelle zum Spawnen von gegnern mit position
    void Start() {
        this.RemotePlayer = GameObject.FindGameObjectWithTag("Player");
        this.RemoteEnemy = GameObject.FindGameObjectWithTag("Enemy");
        this.InitialAiPosition = gameObject.transform.position;
        this.AiPathInstance = gameObject.GetComponent<AIPath>();
        InvokeRepeating("RespawnEnemyIfFallenOff",
                        this.checkForRespawnInterval,
                        this.checkForRespawnInterval);
    }

    void Update() {
        if (DistanceToPlayer() <= maxDistanceToHit) {
            StartCoroutine(HitPlayerAndWait());
        }
    }

    private float DistanceToPlayer() {
        return Vector3.Distance(RemotePlayerPosition, RemoteEnemyPosition);
    }


    private IEnumerator HitPlayerAndWait() {
        // TODO improve
        var directionToPlayer = RemotePlayerPosition - RemoteEnemyPosition;
        directionToPlayer.y = 1f;
        directionToPlayer.Normalize();
        RemotePlayerRigidBody.AddForce(directionToPlayer * 10, ForceMode.Impulse);
        this.CanMove = false;
        yield return new WaitForSeconds(this.inactiveTime);
        this.CanMove = true;
    }

    private void RespawnEnemyIfFallenOff() {
        var y = gameObject.transform.position.y;
        if (y < -20f) {
            gameObject.transform.position = InitialAiPosition;
        }
    }
}
