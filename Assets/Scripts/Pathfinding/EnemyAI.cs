using UnityEngine;
using UnityEngine.AI;


public enum EnemyState {
    WAITING, CHASING_PLAYER, ENGAGING_MAP_TARGET
}

public class EnemyAI : MonoBehaviour
{
    public EnemyState enemyState { get; set; } 
    public float maxDistanceToHit { get; set; } 
    public float hitStrength { get; set; } 
    public float howLongToWait { get; set; } 
    public float checkForRespawnInterval { get; set; } 
    public float predictVelocity { get; set; } 
    public float aggressiveness { get; set; }
    // When enemy is engaging a new map target, pick a position to go to within this radius 
    public float walkRadius { get; set; }
    public float speed { get; set; }

    public float acceleration { get; set;}
    
    
    // on first run, wait longer
    private float remainingWaitSeconds = 0f; 

    private Vector3 OwnPosition { 
        get { return gameObject.transform.position; }
    }

    // When chasing player
    private GameObject localTarget;
    private Vector3 PlayerTargetPosition {
         get { return localTarget.transform.position; } 
    }
    // When not chasing player
    private Vector3 MapTargetPosition;


    private Rigidbody remoteRigid;
    private Vector3 initialPosition;
    private NavMeshAgent navAgent;

    void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        localTarget = player.GetComponent<SyncRemoteObject>().remote_object.gameObject;

        remoteRigid = player.GetComponent<Rigidbody>();

        initialPosition = gameObject.transform.position;
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        navAgent.speed = this.speed;
        navAgent.acceleration = this.acceleration;
        InvokeRepeating("RespawnEnemyIfFallenOff",
                        this.checkForRespawnInterval,
                        this.checkForRespawnInterval);
    }

    public void changeState(EnemyState state) {
        this.enemyState = state;
        switch (state) {
            case EnemyState.CHASING_PLAYER: 
                break;
            case EnemyState.ENGAGING_MAP_TARGET:
                navAgent.destination = getRandomPositionOnNavMesh();
                break;
            case EnemyState.WAITING:
                remainingWaitSeconds += howLongToWait;
                break;
        }
    }

    void Update() {
        switch (this.enemyState) {
            case EnemyState.CHASING_PLAYER: 
                chasePlayer();
                break;
            case EnemyState.ENGAGING_MAP_TARGET:

                engageMapTarget();
                break;
            case EnemyState.WAITING:
                wait();
                break;
        }
    }

    private void wait() {
        // wait for Hit cooldown
        if (remainingWaitSeconds > 0) {
            remainingWaitSeconds -= Time.deltaTime;
        } else {
            if (Random.value <= aggressiveness)
                changeState(EnemyState.CHASING_PLAYER);
            else
                changeState(EnemyState.ENGAGING_MAP_TARGET);
        }
    }

    private void chasePlayer() {
        // tell agent to continue navigating
        navAgent.destination = PlayerTargetPosition + remoteRigid.velocity * predictVelocity;

        // Hit target if close enough
        if (DistanceToPlayer() <= maxDistanceToHit) {
            HitPlayer();
            changeState(EnemyState.WAITING);
        }
    }

    private void engageMapTarget() {
        // Check if enemy has reached map target
        if (!navAgent.pathPending &&
            navAgent.remainingDistance <= navAgent.stoppingDistance &&
            (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f))
        {
            // start wating when map target is reached
            changeState(EnemyState.WAITING);
        }
    }

    private float DistanceToPlayer() {
        return Vector3.Distance(PlayerTargetPosition, OwnPosition);
    }


    private void HitPlayer() {
        var directionToPlayer = PlayerTargetPosition - OwnPosition;
        directionToPlayer.y = 1f;
        directionToPlayer.Normalize();
        remoteRigid.AddForce(directionToPlayer * hitStrength, ForceMode.Impulse);
    }

    private void RespawnEnemyIfFallenOff() {
        var y = gameObject.transform.position.y;
        if (y < -20f) {
            gameObject.transform.position = initialPosition;
        }
    }

    private Vector3 getRandomPositionOnNavMesh() {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1)) {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
