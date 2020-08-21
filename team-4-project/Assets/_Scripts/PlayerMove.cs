using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
//        GetComponent<Rigidbody>().velocity = new Vector3(-3, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(3*Time.deltaTime, 0, 0));
    }
}
