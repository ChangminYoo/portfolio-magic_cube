using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public List<Transform> WayPoints { get; private set; }

    void Start()
    {
        WayPoints = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            WayPoints.Add(transform.GetChild(i));
        }
    }

}
