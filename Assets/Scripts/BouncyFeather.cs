using System;
using UnityEngine;

public class BouncyFeather : MonoBehaviour
{
    // Needs to  be set in-Editor
    public string CollissionTag = "Player";
    public AudioClip CollisionClip;

    public float Bounciness = 2.5f;
    public float CollisionVolume = 30f;

    private AudioSource CollisionAudio;
    private Renderer Render;


    // Start is called before the first frame update
    void Start()
    {
        CollisionAudio = gameObject.AddComponent<AudioSource>();
        Render = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Render.material.color = Vector4.Lerp(Render.material.color, Color.white, Time.deltaTime * 0.3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != CollissionTag) return;

        // play sound
        float vol = collision.relativeVelocity.magnitude / CollisionVolume;
        CollisionAudio.PlayOneShot(CollisionClip, Math.Min(1f, Math.Max(0.01f, vol)));

        // change visually
        Render.material.color = Color.red;

        // reflect off
        Vector3 normal = collision.contacts[0].normal;
        collision.rigidbody.velocity = Vector3.Reflect(collision.relativeVelocity, normal) * Bounciness;
    }
}
