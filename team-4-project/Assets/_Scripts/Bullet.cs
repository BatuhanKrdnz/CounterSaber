using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletType bulletType;

    public float speed;
    public float fireRate;
    public float maxDistance = 500;

    private float m_distanceTraveled;

    private bool isCountered = false;

    public bool IsCountered
    {
        get => isCountered;
        set => isCountered = value;
    }

    void Update()
    {
        if (m_distanceTraveled > maxDistance)
        {
            Destroy(gameObject);
            return;
        }

        if (speed >= 0.000001f)
        {
            var transform1 = transform;
            transform1.position += transform1.forward * (speed * Time.deltaTime);
            m_distanceTraveled += speed * Time.deltaTime;
        }
        else
        {
            Debug.Log("Bullet ======= No Speed");
        }
    }
}