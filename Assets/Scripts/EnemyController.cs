using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    List<GameObject> Enemies;
    // Global enemy settings
    public float maxDistanceToHit = 1.1f;
    public float hitStrength = 20f;
    public float howLongToWait = 2f;
    public float checkForRespawnInterval = 2f;
    public float predictVelocity = 0.1f;
    public EnemyState initialEnemyState = EnemyState.CHASING_PLAYER;
    public float walkRadius = 100f;
    // likeliness of the enemy deciding to chase the player instead of a random position
    public float aggressiveness = 0.5f;

    // TODO: check if those are needed 
    // readable sub-lists based on the enemy state
    List<GameObject> ChasingPlayerEnemies {
        get { return Enemies.FindAll(enemy => enemy.GetComponent<EnemyAI>().enemyState == EnemyState.CHASING_PLAYER); }
    }
    List<GameObject> EngagingMapPositionEnemies {
        get { return Enemies.FindAll(enemy => enemy.GetComponent<EnemyAI>().enemyState == EnemyState.ENGAGING_MAP_TARGET); }
    }
    List<GameObject> WaitingEnemies {
        get { return Enemies.FindAll(enemy => enemy.GetComponent<EnemyAI>().enemyState == EnemyState.WAITING); }
    }
    List<GameObject> NonChasingPlayerEnemies {
        get { return Enemies.FindAll(enemy => enemy.GetComponent<EnemyAI>().enemyState != EnemyState.CHASING_PLAYER); }
    }
    List<GameObject> NonEngagingMapPositionEnemies {
        get { return Enemies.FindAll(enemy => enemy.GetComponent<EnemyAI>().enemyState != EnemyState.ENGAGING_MAP_TARGET); }
    }
    List<GameObject> NonWaitingEnemies {
        get { return Enemies.FindAll(enemy => enemy.GetComponent<EnemyAI>().enemyState != EnemyState.WAITING); }
    }

    void Start()
    {
        Enemies = new List<GameObject>();
    }

    void Update()
    {
        
    }

    public void addEnemy(GameObject enemy)
    {
        initializeEnemy(enemy);
        Enemies.Add(enemy);
    }

    private void initializeEnemy(GameObject enemy)
    {
        EnemyAI enemyAi = enemy.GetComponent<EnemyAI>();
        enemyAi.maxDistanceToHit = maxDistanceToHit;
        enemyAi.hitStrength = hitStrength;
        enemyAi.howLongToWait = howLongToWait;
        enemyAi.checkForRespawnInterval = checkForRespawnInterval;
        enemyAi.predictVelocity = predictVelocity;
        enemyAi.enemyState = initialEnemyState;
        enemyAi.walkRadius = walkRadius;
        enemyAi.aggressiveness = aggressiveness;
    }

    // TODO: check if those are needed
    public void startChasingPlayer(int numberOfEnemies = 1) {
        for (int i = 0; i<numberOfEnemies; i++) {
            if (NonChasingPlayerEnemies.Count > 0) {
                NonChasingPlayerEnemies[0].GetComponent<EnemyAI>()
                                          .changeState(EnemyState.CHASING_PLAYER);
            }
        }
    }

    public void startEngagingRandomMapTarget(int numberOfEnemies = 1) {
        for (int i = 0; i<numberOfEnemies; i++) {
            if (NonEngagingMapPositionEnemies.Count > 0) {
                NonEngagingMapPositionEnemies[0].GetComponent<EnemyAI>()
                                                .changeState(EnemyState.ENGAGING_MAP_TARGET); 
            }
        }
    }

    public void startWatiting(int numberOfEnemies = 1) {
        for (int i = 0; i<numberOfEnemies; i++) {
            if (NonWaitingEnemies.Count > 0) {
                NonWaitingEnemies[0].GetComponent<EnemyAI>()
                                    .changeState(EnemyState.WAITING);
            }
        }
    }
}
