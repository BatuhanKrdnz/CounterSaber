using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameflow : MonoBehaviour
{

    public GameObject Holder;
    
    public Transform laneObj;
    private Vector3 nextLaneSpawn;
    

//    public Transform alienFighter;
//    private Vector3 nextFighterSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
        nextLaneSpawn.y = -14;
        
       
        StartCoroutine(spawnLane());

//        nextFighterSpawn.x = -35;
//        nextFighterSpawn.y = 8;
//        nextFighterSpawn.z = -76;
//        StartCoroutine(spawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spawnLane()
    {
        yield return new WaitForSeconds(2);
        Instantiate(laneObj, nextLaneSpawn, laneObj.rotation);
        nextLaneSpawn.x += 72;
        StartCoroutine(spawnLane());
    }

        //    IEnumerator spawnEnemy()
        //    {
        //        yield return new WaitForSeconds(3);
        //        Instantiate(alienFighter, nextFighterSpawn, alienFighter.rotation);
        //        nextFighterSpawn.x += 85;
        //        StartCoroutine(spawnEnemy());
        //    }
    }
