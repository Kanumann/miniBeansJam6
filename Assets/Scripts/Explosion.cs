using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Explosion : MonoBehaviour
{
    public LayerMask layers;
    public Animator animator;
    public GameObject vfx_explosion;
    public float vfx_duration = 10f;
    public float force = 15f, upwards_modifier = 2f, default_delay = 2f, domino_effect_delay = 2f;
    public string[] trigger_tags;
    public SphereCollider explosion_area, trigger_area;
    public UnityEvent onTriggered, onExploded;
    private bool is_triggered, exploded;
    private GameManager gameManager;

    public void Awake()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        vfx_explosion.gameObject.SetActive(false);
        Activate();
    }

    public void Activate()
    {
        explosion_area.enabled = enabled = is_triggered = exploded = false;
        trigger_area.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (transform.parent == null) transform.parent = gameManager.staticLevelHelper;

    }

    private void OnTriggerEnter(Collider other)
    {
        // Who?
        for (int i = 0; i < trigger_tags.Length; i++)
        {
            if (other.tag == trigger_tags[i]) break;
            else if (i == trigger_tags.Length - 1) return;
        }

        // explode
        TriggerExplosion(default_delay);
    }

    public void TriggerExplosion(float delay)
    {
        // Already triggered?
        if (is_triggered) return;

        // triggered
        is_triggered = true;

        // Switch collider
        explosion_area.enabled = true;
        trigger_area.enabled = false;

        // event
        onTriggered.Invoke();

        // Animation
        animator.SetTrigger("triggered");

        // Sound
        AudioManager.Instance.PlayOneShot("event:/World/Explosion", this.gameObject);

        // delay
        Invoke("Explode", delay);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!is_triggered || !enabled) return;
        else if (is_triggered && enabled) exploded = true;

        if (layers != (layers | (1 << other.gameObject.layer))) return;
        
        Rigidbody phy = other.GetComponent<Rigidbody>();
        if (phy == null) return;
        phy.AddExplosionForce(force, transform.position, explosion_area.radius, upwards_modifier, ForceMode.Impulse);

        // domino effect
        Explosion exp_script = other.GetComponent<Explosion>();
        if (exp_script != null) exp_script.TriggerExplosion(domino_effect_delay);
    }

    private void FixedUpdate()
    {
        if (exploded)
        {
            exploded = false;
            onExploded.Invoke();

            vfx_explosion.transform.SetParent(null, true);
            vfx_explosion.SetActive(true);

            gameObject.SetActive(false);

            Invoke("Despawn", vfx_duration);
        }
    }

    public void Explode()
    {
        enabled = true;
    }

    private void Despawn()
    {
        Destroy(vfx_explosion);
        Destroy(gameObject);
    }
}
