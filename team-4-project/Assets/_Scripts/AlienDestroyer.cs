using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienDestroyer : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0,0,5)*Time.deltaTime);
    }
}
