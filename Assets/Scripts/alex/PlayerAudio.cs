﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    FMOD.Studio.EventInstance instance;
    Rigidbody rb;
    int numberOfEnemies;

    void Awake()
    {
        TryGetComponent<Rigidbody>(out rb);
    }

    void Start()
    {
        AudioManager.Instance.PostEvent("event:/Player/Ball Rolling", out instance, this.gameObject);
        InvokeRepeating("CheckNumberOfEnemies", 5f, 5f);
    }

    void CheckNumberOfEnemies()
    {
        numberOfEnemies = GameObject.FindObjectsOfType<RemoteEnemyBehaviour>().Length;

        switch (numberOfEnemies)
        {
            case 0:
                AudioManager.Instance.SetGlobalParameter("g_musicState", (float)MusicState.Start);
                break;
            case 2:
                AudioManager.Instance.SetGlobalParameter("g_musicState", (float)MusicState.Transition);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        _ = GroundCheck() ? AudioManager.Instance.SetGlobalParameter("g_ballSpeed", rb.velocity.magnitude)
                          : AudioManager.Instance.SetGlobalParameter("g_ballSpeed", 0f);

    }

    bool GroundCheck()
    {
        float distance = 0.7f;
        Vector3 dir = new Vector3(0f, -1f);
        return _ = Physics.Raycast(transform.position, dir, out RaycastHit hit, distance, 1 << 10) ? true : false;
    }

    void OnTriggerEnter(Collider collider)
    {
        //if (collider.tag == "Slowing") AudioManager.Instance.PlayOneShot("event:/World/Trigger1", this.gameObject);
        //if (collider.tag == "Accelerating") AudioManager.Instance.PlayOneShot("event:/World/Trigger2", this.gameObject);

        if (collider.gameObject.CompareTag("AudioTrigger")) AudioManager.Instance.SetGlobalParameter("g_musicStinger", 1f);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "StaticLevel" && col.impulse.magnitude > 10f)
        {
            AudioManager.Instance.PlayOneShot("event:/Player/Land", this.gameObject);
        }
    }

}
