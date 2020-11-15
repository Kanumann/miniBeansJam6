using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneScript : MonoBehaviour
{
    public Transform zone;
    public float y_offset = 0f, zone_spawn_area_radius = 14f, zone_radius = 5f, min_next_zone_distance = 5f, zone_reach_time = 15f;

    private const float DEFAULT_OBJ_RADIUS = 5f;
    private Transform ball;
    public float time;
    private GameManager gameManager;

    private void Start()
    {
        ball = GameObject.FindWithTag("Player").transform;
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }


    void Update()
    {
        time += Time.deltaTime;
        // UI Timer updaten
        if ((ball.position - zone.position).magnitude < DEFAULT_OBJ_RADIUS) OnZoneReached();
        //if (time > zone_reach_time) gameManager.EndGame(); TODO
    }

    void SpawnZone(Vector2 position, float radius)
    {
        zone.transform.localPosition = new Vector3(position.x, y_offset, position.y);
        radius = radius / DEFAULT_OBJ_RADIUS;
        zone.localScale = new Vector3(radius, 1f, radius);
    }

    void OnZoneReached()
    {
        // Reset time
        time = 0f;

        // Calculate new spawn
        Vector2 new_pos = Vector2.zero, current_local_pos = Vector2.zero;
        for (int i = 0; i < 500 && (new_pos - current_local_pos).magnitude - (2 * zone_radius) < min_next_zone_distance; i++)
        {
            new_pos = Random.insideUnitCircle * zone_spawn_area_radius;
            current_local_pos = new Vector2(zone.localPosition.x, zone.localPosition.z);
        }

        // Spawn new
        SpawnZone(new_pos, zone_radius);
    }
}
