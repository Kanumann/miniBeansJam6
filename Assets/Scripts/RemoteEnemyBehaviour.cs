using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RemoteEnemyBehaviour : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject pathFinderAI;
    public VisualEffect vfx_movement_effect;
    public float vfx_speed_modifier;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        gameObject.name = "RemoteEnemy";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (pathFinderAI != null) return;
        Destroy(GetComponent<Rigidbody>());
        transform.parent = gameManager.staticLevelHelper;
        gameObject.layer = 10;
        pathFinderAI = gameManager.RegisterNewEnemy(gameObject);
    }

    private Vector3 last_position;
    private void Update()
    {
        // Dust trail
        vfx_movement_effect.SetFloat("speed", vfx_speed_modifier * (last_position - transform.position).sqrMagnitude);

        // Remember position
        last_position = transform.position;
    }

    private void OnDestroy()
    {
        Debug.Log("Enemy died!");
        Destroy(pathFinderAI);
    }
}
