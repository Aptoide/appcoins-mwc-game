using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour {

    public GameObject[] trails;
    int next = 0;



    void Start()
    {
        //print("estou no start");
            InvokeRepeating("_spawnTrail", 0.1f, 0.1f);
    }

    void _spawnTrail()
    {
        //print("Invocando 1");
        if (GetComponent<Rigidbody2D>().velocity.sqrMagnitude > 25)
        {
            //print("Invocando 2" );
            Instantiate(trails[next], transform.position, Quaternion.identity);
            next = (next + 1) % trails.Length;
        }
    }
}
