using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputEvent press;
    [SerializeField] private InputEvent release;
    [SerializeField] private float counterWindowSeconds = 0.1f;
    
    private bool isSwiping = false;
    private bool canCounter = false;

    private Vector2 lastInputPos;
    private float speed = 0f;
    private Vector2 swipeDirection;

    public bool CanCounter => speed > 0.2f && canCounter;
    public bool IsSwiping => isSwiping;

    public Vector3 GetCounterDirection()
    {
        Vector3 shootDirection = new Vector3(swipeDirection.y, speed/6, -swipeDirection.x);
        return shootDirection;
    }
    
    // Update is called once per frame
    void Update()
    {
        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0) && !isSwiping)
        {
            lastInputPos = Input.mousePosition;
            StartCoroutine(SwipeCoroutine());
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isSwiping = false;
        }
        #endregion
        
        #region Mobile Inputs
        
        if(Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began && !isSwiping)
            {
                lastInputPos = Input.mousePosition;
                StartCoroutine(SwipeCoroutine());
            }
            else if(Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isSwiping = false;
            }
        }
        #endregion
    }

    IEnumerator SwipeCoroutine()
    {
        isSwiping = true;
        canCounter = true;
        yield return new WaitForSeconds(0.05f);
        while (isSwiping)
        {
            Vector2 currentPos = Input.mousePosition;
            swipeDirection = currentPos - lastInputPos;
            speed = swipeDirection.magnitude;
            lastInputPos = currentPos;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(counterWindowSeconds);
        speed = 0f;
        canCounter = false;
    }
}