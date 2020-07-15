using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPosition : MonoBehaviour
{
    public Main mainScript;
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().isKinematic = false;

        //if (mainScript.move)
        //{
        //    GetComponent<Rigidbody2D>().isKinematic = false;
        //}
        //else if (mainScript.move == false)
        //{
        //    GetComponent<Rigidbody2D>().isKinematic = true;
        //}

    }
 

}
