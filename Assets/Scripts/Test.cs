using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject prfb_obj;
    public Transform remote_level;

    // Update is called once per frame
    void Start()
    {
        Vector3 spawn_pos = remote_level.TransformPoint(new Vector3(0, 3f, 0f));
        Debug.Log(spawn_pos);
        GameObject tmp = Instantiate(prfb_obj, spawn_pos, Quaternion.identity, remote_level);
        tmp.name = "LOOOOOOOOOOL";
    }
}
