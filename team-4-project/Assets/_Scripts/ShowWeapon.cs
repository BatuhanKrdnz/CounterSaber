using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWeapon : MonoBehaviour
{

    public GameObject lightSaber;
    public bool showSaber;
    // Start is called before the first frame update
    void Start()
    {
        showSaber = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(showSaber == true)
        {
            lightSaber.SetActive(true);
        }
    }
}
