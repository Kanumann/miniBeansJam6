using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncRemoteObject : MonoBehaviour
{
    public Transform remote_object;
    public Transform reference_space;

    void LateUpdate()
    {
        if (reference_space != null)
        {
            remote_object.localPosition = reference_space.worldToLocalMatrix * transform.position;
            remote_object.localRotation = transform.rotation * reference_space.rotation;
        }
        else
        {
            remote_object.localPosition = transform.localPosition;
            remote_object.localRotation = transform.localRotation;
        }
    }
}
