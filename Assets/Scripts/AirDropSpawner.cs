using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDropSpawner : MonoBehaviour
{
    public GameObject prfb_object;
    public Transform parent;
    public float min_time, max_time, spawn_radius, spawn_height;
    public int max_objects;
    private int current_object_count;
    private float time, next_spawn_time;

    public void OnEnabled()
    {
        // Reset timer
        time = 0f;
        next_spawn_time = Random.Range(min_time, max_time);
    }

    void FixedUpdate()
    {
        // Random timer for spawns
        time += Time.deltaTime;
        if (time >= next_spawn_time)
        {
            Spawn();
            OnEnabled();
        }
    }

    void Spawn()
    {
        // Calculate spawn position
        Vector2 random_2d_pos = Random.insideUnitCircle * spawn_radius;
        Vector3 spawn_pos = new Vector3(random_2d_pos.x, spawn_height, random_2d_pos.y);

        // Spawn object
        GameObject obj = Instantiate(prfb_object, spawn_pos, Quaternion.identity, parent);
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
