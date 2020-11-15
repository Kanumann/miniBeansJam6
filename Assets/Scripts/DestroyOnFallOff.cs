using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyOnFallOff : MonoBehaviour
{
    public float destroy_heigth = -20;
    public UnityEvent onFallOff, onDestroy;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < destroy_heigth)
        {
            onFallOff.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        onDestroy.Invoke();
    }
}
