using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Default,
    Laser
}

public class Shooter : MonoBehaviour
{
    public Transform shooterTransform;

    public GameObject[] bulletPrefabs;

    public BulletType m_bulletType;

    private GameObject m_selectedBullet;


    private void Start()
    {
        if (shooterTransform == null)
            shooterTransform = transform;

        AssignBulletPrefabForType();
    }

    public void AssignBulletType(BulletType bulletType)
    {
        this.m_bulletType = bulletType;
        AssignBulletPrefabForType();
    }

    private void AssignBulletPrefabForType()
    {
        foreach (GameObject bulletPrefab in bulletPrefabs)
        {
            if (bulletPrefab.GetComponent<Bullet>().bulletType == m_bulletType)
            {
                m_selectedBullet = bulletPrefab;
                break;
            }
        }

        if (m_selectedBullet == null)
            Debug.LogWarning("Shooter ======  No bullet prefab found with bulletType " + m_bulletType);
    }

    public void Shoot(Vector3 destination)
    {
        if (m_selectedBullet != null)
        {
            Vector3 source = shooterTransform.position;
            Vector3 direction = destination - source;
            Quaternion rotation = Quaternion.LookRotation(direction);
            Instantiate(m_selectedBullet, source, rotation);
        }
        else
        {
            Debug.LogWarning("Shooter ====== Bullet Prefab Not Assigned");
        }
    }
    
    public void ShootDirection(Vector3 direction)
    {
        if (m_selectedBullet != null)
        {
            
            Quaternion rotation = Quaternion.LookRotation(direction);
            Instantiate(m_selectedBullet, shooterTransform.position, rotation);
        }
        else
        {
            Debug.LogWarning("Shooter ====== Bullet Prefab Not Assigned");
        }
    }
}