﻿using System;
using UnityEngine;

public class BouncyFeather : MonoBehaviour
{
    // Needs to  be set in-Editor
    public string CollissionTag = "Player";

    public float Bounciness = 2.5f;
    public float CollisionVolume = 30f;

    [SerializeField] [FMODUnity.EventRef]
    private string audioEvent = "event:/World/BouncyPlate";


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != CollissionTag) return;

        // play sound
        float vol = collision.relativeVelocity.magnitude / CollisionVolume;
        AudioManager.Instance.PlayOneShotParameter(audioEvent, "l_volume", vol, this.gameObject);

        // reflect off
        Vector3 normal = collision.contacts[0].normal;
        collision.rigidbody.velocity = Vector3.Reflect(collision.relativeVelocity, normal) * Bounciness;
    }
}
