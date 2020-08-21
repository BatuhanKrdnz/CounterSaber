using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpeedOption();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpeedOption()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(4, 0, 0);
    }
}
