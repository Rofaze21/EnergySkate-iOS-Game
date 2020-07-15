using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Gui;

// keep the lines
public class dontDestroy : MonoBehaviour
{
    public Main main;
    public LeanButton loseConditionQuitButton;
    public LeanButton winQuitButton;
    public LeanButton pauseQuitButton;

    // Start is called before the first frame update
    void Start()
    {
        loseConditionQuitButton.OnClick.AddListener(quitPressed);
        winQuitButton.OnClick.AddListener(quitPressed);
        pauseQuitButton.OnClick.AddListener(quitPressed);

    }

    void quitPressed()
    {
        main.destroyLines();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] lines = GameObject.FindGameObjectsWithTag("Line");
        foreach (GameObject line in lines)
        {
            DontDestroyOnLoad(line);
        }
        if (lines.Length != 0)
        {
            main.initialStart = false;
        }
        
    }
}
