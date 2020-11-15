using UnityEngine;
using UnityEngine.AI;


// @TODO public schnittstelle zum Spawnen von gegnern mit position

public class EnemyAI : MonoBehaviour
{
    public float maxDistanceToHit = 1.1f;
    public float hitStrength = 20f;
    public float inactiveTime = 2f;
    public float checkForRespawnInterval = 2f;
    public float predictVelocity = 0.1f;

    private Vector3 OwnPosition { 
        get { return gameObject.transform.position; }
    }

    private GameObject localTarget;
    private Vector3 TargetPosition {
         get { return localTarget.transform.position; } 
    }

    private GameObject player;

    // TODO(Jonas): Overwrite maxDistanceToHit depending on player 
    //            :and enemy size
    private Rigidbody remoteRigid;
    private Vector3 initialPosition;
    private NavMeshAgent navAgent;

    private float waitSeconds = 3f; // on first run, wait longer

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        localTarget = player.GetComponent<SyncRemoteObject>().remote_object.gameObject;

        remoteRigid = player.GetComponent<Rigidbody>();

        initialPosition = gameObject.transform.position;
        navAgent = gameObject.GetComponent<NavMeshAgent>();

        InvokeRepeating("RespawnEnemyIfFallenOff",
                        this.checkForRespawnInterval,
                        this.checkForRespawnInterval);
    }

    void Update() {
        // wait for Hit cooldown
        if (waitSeconds > 0)
        {
            waitSeconds -= Time.deltaTime;
            return;
        }

        // tell agent to continue navigating
        navAgent.destination = TargetPosition + remoteRigid.velocity * predictVelocity;

        // Hit target if close enough
        if (DistanceToPlayer() <= maxDistanceToHit)
        {
            HitPlayer();
            waitSeconds = inactiveTime;
        }
    }

    private float DistanceToPlayer() {
        return Vector3.Distance(TargetPosition, OwnPosition);
    }


    private void HitPlayer() {
        var directionToPlayer = TargetPosition - OwnPosition;
        directionToPlayer.y = 1f;
        directionToPlayer.Normalize();
        remoteRigid.AddForce(directionToPlayer * hitStrength, ForceMode.Impulse);
        AudioManager.Instance.PlayOneShot("event:/World/Hit", player);
    }

    private void RespawnEnemyIfFallenOff() {
        var y = gameObject.transform.position.y;
        if (y < -20f) {
            gameObject.transform.position = initialPosition;
        }
    }
}
