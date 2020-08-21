using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    public bool m_isPaused = false;

    public GameObject m_pausePanel;

    [SerializeField] private SwipeDifficulty swipeDifficulty = SwipeDifficulty.Easy;
    [SerializeField] private TimingDifficulty timingDifficulty = TimingDifficulty.Easy;

    [SerializeField] private int m_MaxEnemyCount = 3;
    [SerializeField] private GameObject m_EnemyHolder = null;
    
    [SerializeField] private GameObject m_EnemyPrefab = null;

    private Vector3 enemySpawnOffset = new Vector3(-50, 0, 0);

    private Player player;
    private List<Enemy> enemyList = new List<Enemy>();

    public Player Player
    {
        get => player;
        set
        {
            if (player != null && value != null)
            {
                Debug.LogWarning("GameManager ===== There is already a player assigned," +
                                 " assignment of new player failed ");
            }
            else
                player = value;
        }
    }

    public TimingDifficulty TimingDifficulty => timingDifficulty;

    private void Start()
    {
        m_pausePanel.SetActive(false);

        StartGame();
    }

    public void StartGame()
    {
        for (int i = 0; i < m_MaxEnemyCount; i++)
        {
            StartCoroutine(SpawnEnemyCoroutine(enemySpawnOffset));
        }
    }
    
    public void RegisterEnemy(Enemy enemy)
    {
        enemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
        if (enemyList.Count < m_MaxEnemyCount)
        {
            StartCoroutine(SpawnEnemyCoroutine(new Vector3(-20,0,0)));
        }
    }

    public Enemy GetEnemyInDirection(Vector3 direction, Vector3 source)
    {
        switch (swipeDifficulty)
        {
            case SwipeDifficulty.Easy:
                return GetEnemyInDirectionEasy(direction, source);
            case SwipeDifficulty.Medium:
                return GetEnemyInDirectionMedium(direction, source);
            case SwipeDifficulty.Hard:
                return GetEnemyInDirectionHard(direction, source);
        }

        return null;
    }

    private Enemy GetEnemyInDirectionEasy(Vector3 direction, Vector3 source)
    {
        float minAngleToMatch = 15;
        float minAngle = minAngleToMatch;
        Enemy result = null;
        foreach (Enemy enemy in enemyList)
        {
            Vector3 curDirection = enemy.transform.position - source;
            curDirection = Vector3.ProjectOnPlane(curDirection, Vector3.up);
            direction = Vector3.ProjectOnPlane(direction, Vector3.up);
            float angleBetween = Vector3.Angle(curDirection, direction);
            if (angleBetween < minAngleToMatch)
            {
                if (angleBetween < minAngle)
                {
                    minAngle = angleBetween;
                    result = enemy;
                }
            }
        }

        return result;
    }

    private Enemy GetEnemyInDirectionMedium(Vector3 direction, Vector3 source)
    {
        float minAngleToMatch = 7;
        float minAngle = minAngleToMatch;
        Enemy result = null;
        foreach (Enemy enemy in enemyList)
        {
            Vector3 curDirection = enemy.transform.position - source;
            curDirection = Vector3.ProjectOnPlane(curDirection, Vector3.up);
            direction = Vector3.ProjectOnPlane(direction, Vector3.up);
            float angleBetween = Vector3.Angle(curDirection, direction);
            if (angleBetween < minAngleToMatch)
            {
                if (angleBetween < minAngle)
                {
                    minAngle = angleBetween;
                    result = enemy;
                }
            }
        }

        return result;
    }

    private Enemy GetEnemyInDirectionHard(Vector3 direction, Vector3 source)
    {
        float minAngleToMatch = 2;
        float minAngle = minAngleToMatch;
        Enemy result = null;
        foreach (Enemy enemy in enemyList)
        {
            Vector3 curDirection = enemy.transform.position - source;
            curDirection = Vector3.ProjectOnPlane(curDirection, Vector3.up);
            direction = Vector3.ProjectOnPlane(direction, Vector3.up);
            float angleBetween = Vector3.Angle(curDirection, direction);
            if (angleBetween < minAngleToMatch)
            {
                if (angleBetween < minAngle)
                {
                    minAngle = angleBetween;
                    result = enemy;
                }
            }
        }

        return result;
    }

    public void TogglePause()
    {    
        m_isPaused = !m_isPaused;

        if (m_pausePanel)
        {
            m_pausePanel.SetActive(m_isPaused);

            Time.timeScale = (m_isPaused) ? 0 : 1;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("BatuhanScene");
    }

    IEnumerator SpawnEnemyCoroutine(Vector3 offset)
    {
        int sec = Random.Range(1, 7);
        yield return new WaitForSeconds(sec);
        SpawnNewEnemy(offset);
    }
    
    private void SpawnNewEnemy(Vector3 offset)
    {
        if (m_EnemyPrefab != null)
        {
            int x = 0;
            int z = 0;
            
            while (true)
            {
                z = Random.Range(-6, 7);
                x = Random.Range(0, 11);
                
                bool isEqual = false;
                
                foreach (Enemy curEnemy in enemyList)
                {
                    float xPos = curEnemy.transform.position.x;
                    float zPos = curEnemy.transform.position.z;
                    if ((xPos - 2 < x && xPos + 2 > x)||
                        (zPos - 2 < z && zPos + 2 > z))
                    {
                        isEqual = true;
                        break;
                    }
                }

                if(!isEqual)
                    break;
            }

            float fireRate = Random.Range(0.1f, 1f);

            Transform parent = m_EnemyHolder.transform;
            Vector3 relativeLocation = new Vector3(x, 0, z);
            GameObject enemyObject = Instantiate(m_EnemyPrefab,
                 relativeLocation + parent.transform.position + offset,
                Quaternion.identity,
                parent);
            enemyObject.transform.parent = m_EnemyHolder.transform;
            
            Enemy enemy = enemyObject.GetComponent<Enemy>();

            enemy.SetTarget(player.transform);
            enemy.SetFireRate(fireRate);
            enemy.MoveToLocation(relativeLocation);
        }
        else
        {
            Debug.LogWarning("GameManager ======= Enemy prefab is null!!!!");
        }
    }
}