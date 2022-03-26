using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    float lifeTime = 5.0f;
    float currentTime = 0;
    void Start()
    {
        currentTime = Time.time;
    }

    void Update()
    {
        if (Time.time - currentTime > lifeTime)
        {
            ObjectPoolManager.Instance.ReleaseObject(gameObject);
        }
    }
}
