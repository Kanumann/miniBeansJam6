using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDropSpawner : MonoBehaviour
{
    public GameObject prfb_object;
    public float min_time = 5f, max_time = 15f, spawn_radius = 18f, spawn_height = 20;
    public int max_objects = 3;
    private int current_object_count;
    private float time, next_spawn_time;

    public void Reset()
    {
        // Reset timer
        time = 0f;
        if (min_time >= max_time) next_spawn_time = min_time;
        else next_spawn_time = Random.Range(min_time, max_time);
    }

    private void OnEnable()
    {
        Reset();
    }

    void FixedUpdate()
    {
        // Random timer for spawns
        time += Time.deltaTime;
        if (time >= next_spawn_time)
        {
            Spawn();
            Reset();
        }
    }

    void Spawn()
    {
        // Calculate spawn position
        Vector2 random_2d_pos = Random.insideUnitCircle * spawn_radius;
        Vector3 spawn_pos = new Vector3(random_2d_pos.x, spawn_height, random_2d_pos.y);

        // Spawn object
        GameObject obj = Instantiate(prfb_object, spawn_pos, Quaternion.identity);
        obj.GetComponent<DestroyOnFallOff>().onDestroy.AddListener(OnObjectDestroyed);
        current_object_count++;

        // Check if spawn limit is reached
        if (enabled && current_object_count >= max_objects) enabled = false;
    }

    void OnObjectDestroyed()
    {
        // Check if spawn limit is reached
        current_object_count--;
        if (!enabled && current_object_count < max_objects) enabled = true;
    }
}
