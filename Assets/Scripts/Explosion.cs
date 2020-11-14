using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Explosion : MonoBehaviour
{
    public float force, upwards_modifier, delay;
    public string[] trigger_tags;
    public SphereCollider explosion_area, trigger_area;
    public UnityEvent onTriggered, onExploded;
    private bool is_triggered, exploded;

    public void Start()
    {
        Activate();
    }

    public void Activate()
    {
        explosion_area.enabled = enabled = is_triggered = exploded = false;
        trigger_area.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Already triggered?
        if (is_triggered) return;

        // Who?
        for (int i = 0; i < trigger_tags.Length; i++)
        {
            if (other.tag == trigger_tags[i]) break;
            else if (i == trigger_tags.Length - 1) return;
        }

        // triggered
        is_triggered = true;

        // Switch collider
        explosion_area.enabled = true;
        trigger_area.enabled = false;

        // Event
        onTriggered.Invoke();

        // Delay
        Invoke("Explode", delay);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!is_triggered || !enabled) return;
        exploded = true;

        Rigidbody phy = other.GetComponent<Rigidbody>();
        if (phy == null) return;
        phy.AddExplosionForce(force, transform.position, explosion_area.radius, upwards_modifier, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        if (exploded)
        {
            exploded = false;
            Destroy(gameObject);

            // Event
            onExploded.Invoke();
        }
    }

    public void Explode()
    {
        enabled = true;
    }
}
