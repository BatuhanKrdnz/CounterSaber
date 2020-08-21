using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform m_target;
    [SerializeField] private Vector3 m_targetOffset = new Vector3(0, 0, 0);

    [SerializeField] private GameObject m_explosionVfx = null;
    [SerializeField] private BulletType m_bulletType = BulletType.Default;

    [SerializeField, Range(0.1f, 1)] private float m_fireRate = 0.1f;

    private GameManager m_gameManager;
    private Shooter m_shooter;

    private Vector3 m_destination;

    [SerializeField] private bool shouldFire = false;

    public void SetFireRate(float fireRate)
    {
        if (fireRate < 0.1f)
            m_fireRate = 0.1f;
        else if (fireRate > 1f)
            m_fireRate = 1f;
        else
            m_fireRate = fireRate;
    }

    public void SetTarget(Transform target)
    {
        m_target = target;
    }

    public void StopFire()
    {
        shouldFire = false;
    }

    public void StartFire()
    {
        shouldFire = true;
        StartCoroutine(ShootingCoroutine());
    }

    public void MoveToLocation(Vector3 destination)
    {
        m_destination = destination;
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        Vector3 source = transform.localPosition;
        float t = 0f;
        float time = 2f;

        while (Vector3.Distance(m_destination, transform.localPosition) > 0.01f)
        {
            t += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(source, m_destination, t / time);
            yield return null;
        }

        transform.localPosition = m_destination;

        yield return new WaitForSeconds(0.5f);

        StartFire();
    }

    private void Start()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_shooter = GetComponent<Shooter>();

        if (m_gameManager == null)
            Debug.LogWarning("Enemy ======= GameManager not found !!");
        else if (m_shooter == null)
            Debug.LogWarning("Enemy ====== Shooter component not found !!");
        else
        {
            m_shooter.AssignBulletType(m_bulletType);
            StartCoroutine(ShootingCoroutine());
        }

        m_gameManager.RegisterEnemy(this);
    }

    IEnumerator ShootingCoroutine()
    {
        while (shouldFire)
        {
            yield return new WaitForSeconds(0.5f / m_fireRate);
            m_shooter.Shoot(m_target.position + m_targetOffset);
        }
    }

    private void Update()
    {
        Quaternion rotation = Quaternion.LookRotation(m_target.position - transform.position);
        transform.localRotation = rotation;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Bullet bullet = collider.GetComponent<Bullet>();
        if (bullet != null && bullet.IsCountered)
        {
            Destroy(bullet.gameObject);
            DestroyThis();
        }
    }

    private void DestroyThis()
    {
        if (m_explosionVfx != null)
        {
            Destroy(gameObject, 0.01f);
            GameObject vfx = Instantiate(m_explosionVfx, transform.position, Quaternion.identity);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (m_gameManager != null)
            m_gameManager.UnregisterEnemy(this);
    }
}