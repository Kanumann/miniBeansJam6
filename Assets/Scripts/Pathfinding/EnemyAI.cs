using UnityEngine;
using UnityEngine.AI;


public enum EnemyState {
    WAITING, CHASING_PLAYER, ENGAGING_MAP_TARGET
}

public class EnemyAI : MonoBehaviour
{
    // TODO(Jonas): Overwrite maxDistanceToHit depending on player and enemy size
    public float maxDistanceToHit = 1.1f;
    public float hitStrength = 20f;
    public float howLongToWait = 2f;
    public float checkForRespawnInterval = 2f;
    public float predictVelocity = 0.1f;
    public EnemyState enemyState { get; private set; } 

    // When enemy is engaging a new map target, pick a position to go to within this radius 
    public float walkRadius = 10f;
    
    // on first run, wait longer
    public float remainingWaitSeconds = 3f; 

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

        InvokeRepeating("RespawnEnemyIfFallenOff",
                        this.checkForRespawnInterval,
                        this.checkForRespawnInterval);
    }

    public void changeState(EnemyState state) {
        switch (state) {
            case EnemyState.CHASING_PLAYER: 
                break;
            case EnemyState.ENGAGING_MAP_TARGET:
                MapTargetPosition = getRandomPositionOnNavMesh(); 
                break;
            case EnemyState.WAITING:
                remainingWaitSeconds += howLongToWait; 
                break;
        }
        this.enemyState = state;
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
                return;
        }
    }

    private void wait() {
        // wait for Hit cooldown
        if (remainingWaitSeconds > 0) {
            remainingWaitSeconds -= Time.deltaTime;
        } else {
            // TODO(Jonas): chase player or go to random location based on what condition?
            changeState(EnemyState.CHASING_PLAYER);
        }
    }

    private void chasePlayer() {
        // tell agent to continue navigating
        navAgent.destination = PlayerTargetPosition + remoteRigid.velocity * predictVelocity;

        // Hit target if close enough
        if (DistanceToPlayer() <= maxDistanceToHit) {
            HitPlayer();
            remainingWaitSeconds = howLongToWait;
            changeState(EnemyState.WAITING);
        }
    }

    private void engageMapTarget() {
        navAgent.destination = MapTargetPosition;
        // start wating when map target is reached
        if (navAgent.pathStatus == NavMeshPathStatus.PathComplete)
            changeState(EnemyState.WAITING);
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
