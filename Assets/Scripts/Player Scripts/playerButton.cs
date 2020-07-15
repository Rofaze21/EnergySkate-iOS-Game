using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerButton : MonoBehaviour
{
    public Rigidbody2D lofiRB;
    public GameObject lofigirl;
    public Camera cam1;
    public Camera cam2;

    // ac
    public Animator anim;

    public Button playButton;
    // move forward when play



    // to get .y 
    public GameObject ground;

    // Main script
    public Main main;
    public Stuck stuck;
    public Backward backward;

    public GameObject lineCreator;


    public GameObject playerCanvas;

 
    public GameObject changeCursor;
    public GameObject raycastPointer;

    public skateSound skateSoundScript;

    private void Start()
    {
        cam1.enabled = true;
        cam2.enabled = false;
        anim = lofigirl.GetComponent<Animator>();
        playButton.onClick.AddListener(playButtonClicked);
    }

    void Update()
    {
        


    }
    private void startGame()
    {
        anim.SetTrigger("Play");
        lofiRB.bodyType = RigidbodyType2D.Dynamic;
        raycastPointer.GetComponent<CursorChangingScript>().enabled = false; // Turn off script that decides weather to use draw, erase, or pointer cursor
        cam1.enabled = false; 
        cam2.enabled = true;
        playerCanvas.SetActive(true);
        //changeCursor.GetComponent<keepPointer>().enabled = true; // Keep pointer cursor on during play mode
        skateSoundScript.playModeActivated = true;
        lineCreator.SetActive(false);// Disable line drawing


    }


    void playButtonClicked()
    {
        startGame();
        stuck.initPos = transform.position;
        backward.initPos = transform.position;
        main.move = true;
       
        lineCreator.SetActive(false); //Disable LineCreator Script
    }
    
    
    



}
