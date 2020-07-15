using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialScript : MonoBehaviour
{
    // When first line is drawn, display next part of the tutorial
    public bool lineFound = false;

    public GameObject enableAfterFirstDraw;
    public List<GameObject> disableAfterFirstDraw;

    void Start()
    {
        //Repeat function to check for line
        InvokeRepeating("checkForLine", 5f, 3f);

    }

    // Update is called once per frame
    void Update()
    {
        if(lineFound == true)
        {
            EnableItems();
            DisableItems();
        }
    }

    void checkForLine()
    {
        GameObject[] lines  = GameObject.FindGameObjectsWithTag("Line");
        if(lines.Length >= 1)
        {
            lineFound = true;
            
        }
    }

    void EnableItems()
    {
        enableAfterFirstDraw.SetActive(true);
        
    }

    void DisableItems()
    {
        foreach (GameObject item in disableAfterFirstDraw)
        {
            item.SetActive(false);

        }
    }
}
