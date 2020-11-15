using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Transform ball;
    private Vector3 initial_position;
    private Rigidbody ball_phy;
    private EnemySpawner EnemySpawnerInstance;

    public Transform staticLevelHelper;

    public GameObject UIScriptHolder;

    public AirDropSpawner spawner1;

    public IngameUI ingameUI;

    private void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Player").transform;
        initial_position = ball.position;
        ball_phy = ball.GetComponent<Rigidbody>();
        EnemySpawnerInstance = GetComponent<EnemySpawner>();

        // get ingame ui control script
        ingameUI = UIScriptHolder.GetComponent<IngameUI>();

        StartGame();
    }

    public GameObject RegisterNewEnemy(GameObject newRemoteEnemy)
    {
        return EnemySpawnerInstance.AddAiToRemoteEnemy(newRemoteEnemy);
    }

    void Update()
    {
        if (ball.transform.position.y < -20f)
        {
            ball_phy.velocity = Vector3.zero;
            ball.position = initial_position;
        }
    }


    //////////////////////////////
    // GAME FLOW
    //////////////////////////////


    /// <summary>
    /// Delete all enemys, etc. Open score screen
    /// </summary>
    public void EndGame()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(enemy);

        spawner1.enabled = false;

        ingameUI.Running = false;
        ingameUI.ShowIGMenu(false);
    }


    public void StartGame()
    {
        ingameUI.Reset();
        ingameUI.ShowIGMenu(true);
        ingameUI.Running = true;

        spawner1.enabled = true;

    }
}
