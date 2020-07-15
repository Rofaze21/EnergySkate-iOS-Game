using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class erase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        // replace onApplicationQuit
        PlayerPrefs.DeleteKey("initialStart");
        PlayerPrefs.DeleteKey("1");
        PlayerPrefs.DeleteKey("2");
        PlayerPrefs.DeleteKey("3");
        PlayerPrefs.DeleteKey("4");
        PlayerPrefs.DeleteKey("5");
    }
}
