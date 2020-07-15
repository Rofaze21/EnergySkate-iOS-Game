using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCircle : MonoBehaviour
{

    public Camera cam1;
    public Camera cam2;

    public GameObject self; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(cam1.enabled == false)
        {
            self.SetActive(false);
        } else
        {
            self.SetActive(true);

        }



    }
}
