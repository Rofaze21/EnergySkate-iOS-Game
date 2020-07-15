using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableDrawing : MonoBehaviour
{

    public GameObject LineCreator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void stopScript()
    {

        LineCreator.SetActive(false);
    }
    public void startScript()
    {

        LineCreator.SetActive(true);
    }
}
