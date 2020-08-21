using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    // Rigidbody component
    public Rigidbody rb;

    public float Force = 1500f;



    // Better for mess with physics
    void FixedUpdate()
    {
        rb.AddForce(0, 0, -Force * Time.deltaTime);
    }
}
