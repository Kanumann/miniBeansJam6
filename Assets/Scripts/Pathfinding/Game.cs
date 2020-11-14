using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating("UpdateAstarGraph", 2, 2);
    }

    void Update()
    {
    }

    void UpdateAstarGraph() {
        Debug.LogWarning("Updating Astar Graph");
        AstarPath.active.Scan();
        Debug.LogWarning("Updating Astar Graph Done!");
    }
}
