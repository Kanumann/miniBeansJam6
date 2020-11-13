using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyFeather : MonoBehaviour
{

    public GameObject[] Targets;
    public float Bounciness = 2.5f;
    public float CollisionVolume = 30f;
    // Needs to  be set in-Editor
    public AudioClip CollisionClip;


    // Representing a runtime audio player
    private AudioSource CollisionAudio;

    // representing "Targets" Rigidbodys for saving performance
    private Rigidbody[] Bodys;


    // Start is called before the first frame update
    void Start()
    {
        CollisionAudio = gameObject.AddComponent<AudioSource>();

        Bodys = new Rigidbody[Targets.Length];
        for (int i = 0; i < Targets.Length; i++)
        {
            Bodys[i] = Targets[i].GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;

        for (int i = 0; i < Targets.Length; i++)
        {
            GameObject obj = Targets[i];

            if (collision.gameObject != obj)
                continue;

            float vol = collision.relativeVelocity.magnitude / CollisionVolume;
            CollisionAudio.PlayOneShot(CollisionClip, Math.Min(1f, Math.Max(0.01f, vol)));

            Bodys[i].velocity = Vector3.Reflect(collision.relativeVelocity, normal) * Bounciness;
        }
    }
}
