using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoveCamera : MonoBehaviour
{
    public float speed = 0.8f;

    public Scrollbar scrollbar;
    public GameObject startBuilding;
    public GameObject endBuilding;

    public GameObject lineScript;
    public float scrollBarMaxValue = 1;
    // Start is called before the first frame update
    void Start()
    {
        scrollbar.onValueChanged.AddListener((float val) => ScrollbarCallback(val));

        //scrollbar.OnDrag;
    }


    // Update is called once per frame
    void Update()
    {
        ScrollbarCallback(scrollbar.value);
      
    }

    public void ScrollbarCallback(float val)
    {
        var value = scrollbar.value * endBuilding.transform.position.x * speed;


        if (scrollbar.value >= scrollBarMaxValue)
        {
            scrollbar.value = scrollBarMaxValue;

        }
        if (value < 0) // Constrain the scroll value to the end of the end building
        {
            value = 0;
            scrollbar.value = 0;

        }
        transform.position = new Vector3(value, transform.position.y, transform.position.z);
    }

    public void stopScripts()  // when scrollbar dragged, stop drawing from happening
    {
        lineScript.SetActive(false);
    }
    public void startScripts() // when scrollbar done being dragged, start drawing again
    {
        lineScript.SetActive(true);
    }
}


