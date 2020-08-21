using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int playerHp = 3;

    private InputManager inputManager;
    private GameManager gameManager;
    private Animator anim;

    private bool isActive = true;

    private bool canBlock = false;
    private bool canCounter = false;
    private Vector3 counterDirection;

    private float counterBeforeWindow = 0f;
    private float counterAfterWindow = 0f;

    public void StartPlayer()
    {
        isActive = true;
    }

    public void StopPlayer()
    {
        isActive = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
        anim = FindObjectOfType<Animator>();
        gameManager.Player = this;
        SetDifficulty();
        if (inputManager != null)
        {
            StartCoroutine(HitAnimationListenerCoroutine());
        }
        else
        {
            Debug.LogWarning("InputManager is null!!!!");
        }
    }

    private void SetDifficulty()
    {
        switch (gameManager.TimingDifficulty)
        {
            case TimingDifficulty.Easy:
                counterBeforeWindow = 0f;
                counterAfterWindow = 0.1f;
                break;
            case TimingDifficulty.Medium:
                counterBeforeWindow = 0.1f;
                counterAfterWindow = 0.2f;
                break;
            case TimingDifficulty.Hard:
                counterBeforeWindow = 0.25f;
                counterAfterWindow = 0.3f;
                break;
        }
    }

    IEnumerator HitAnimationListenerCoroutine()
    {
        bool counterOccured = false;
        while (isActive)
        {
            if (inputManager.IsSwiping)
            {
                if (!counterOccured)
                    canBlock = true;
                
                if (inputManager.CanCounter && !counterOccured)
                {
                    //counter bullet
                    counterOccured = true;
                    canBlock = false;
                    counterDirection = inputManager.GetCounterDirection();
                    if (counterDirection.z >= 0)
                    {
                        anim.SetTrigger("HitRight");
                    }
                    else
                    {
                        anim.SetTrigger("HitLeft");
                    }

                    yield return new WaitForSeconds(counterBeforeWindow);
                    canCounter = true;

                    yield return new WaitForSeconds(0.75f - (counterAfterWindow + counterBeforeWindow));
                    canCounter = false;
                }
            }
            else
            {
                counterOccured = false;
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (canCounter)
        {
            //counter
            Enemy hitEnemy = gameManager.GetEnemyInDirection(counterDirection, collider.transform.position);
            if (hitEnemy != null)
            {
                Debug.Log("Enemy hit at position : " + hitEnemy.transform.position);
                counterDirection = hitEnemy.transform.position - collider.transform.position;
            }

            Quaternion rotation = Quaternion.LookRotation(counterDirection);
            collider.GetComponent<Bullet>().IsCountered = true;
            collider.GetComponent<Transform>().rotation = rotation;
        }

        else if (canBlock)
        {
            //only block
            Destroy(collider.gameObject);
        }

        else
        {
            //hit by bullet
            Debug.Log("Player hit by bullet!!");
            Destroy(collider.gameObject);
        }
    }

    private void OnDestroy()
    {
        gameManager.Player = null;
    }
}