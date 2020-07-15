using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    // scripts
    public Main main;
    public GameObject winCanvas;
    public GameObject loseCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void win()
    {
        bool lost = main.hasPopup(false);
        
        if (!winCanvas.activeSelf && !lost)
        {
            main.playAnim("Win", "Win");
            StartCoroutine(main.popUp(winCanvas));
        }
        
    }
}
