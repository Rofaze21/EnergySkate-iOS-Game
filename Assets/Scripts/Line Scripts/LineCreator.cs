using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LineCreator : MonoBehaviour
{

    // Defines line which will be duplicated repetetively 
    public GameObject linePrefab;

    // Current line that is being drawn 
    Line activeLine;

    // draw tools
    public Button drawButton;
    public Button eraseButton;
    public Button eraseAllButton;
    public Button playButton;

    // gameobjects that highlight the draw and erase buttons
    public GameObject drawCircle;
    public GameObject eraseCircle;

    // current state (draw, erase, or erase all)
    public string currentState = "draw";

    // Game object used to constrain the drawing within the buildings
    public GameObject drawConstrainer;

    // Line Smoothing Script
    public lineSmoother smoother;


    // Audio clips for erase and draw
    public List<AudioClip> audioClips;

    // Audio Source to play sound for erase and draw
    public AudioSource audioSource;

    // Used to track if mouse has moved across the scene
    Vector3 lastMouseCoordinate = Vector3.zero;


    public int prevColorIndex = -1;
    private void Start()
    {
        // add event listeners to buttons 
        drawButton.onClick.AddListener(drawButtonPressed);
        eraseButton.onClick.AddListener(eraseButtonPressed);
        eraseAllButton.onClick.AddListener(eraseAllButtonPressed);

        audioSource.Play();

    }

   
    void Update()
    {
        PlayDrawAndEraseSounds();


        //if (Input.GetMouseButtonDown(0))
        //{
        //    audioSource.UnPause();
        //}


        if (currentState == "draw" ){


            // Highlight draw tool
            drawCircle.GetComponent<SpriteRenderer>().enabled = true; 
            eraseCircle.GetComponent<SpriteRenderer>().enabled = false; 

            // Enable draw constrainer game object's collider
            drawConstrainer.GetComponent<Collider2D>().enabled = true;
            
           


        // Draw the line's first point on the screen(as long as it is in the drawing constrainer bounds) and set active line to that line

        if (Input.GetMouseButtonDown(0))
        {
               


                RaycastHit2D raycastHit = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.01f, Vector2.zero);

                if (raycastHit.collider != null)
                {
                   

                    //Determine line color
                    GameObject lineGO = Instantiate(linePrefab);
                    activeLine = lineGO.GetComponent<Line>();


                  


                    // if ray hits anything outside of the drawing constrainer, don't draw
                    if (raycastHit.collider.gameObject.transform.name != "Drawing Constrainer") 
                    {
                        return;
                    }

                }


            }



        if (Input.GetMouseButtonUp(0))
        {
                activeLine = null;


         }



            // Add more points to previous points making a line 
            if (activeLine != null)
            {

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit;

                hit = Physics2D.Raycast(mousePos, Vector2.zero);


                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.transform.name == "Drawing Constrainer" )
                    {

                        activeLine.UpdateLine(mousePos);


                        }
                    }
                }

            


        } else if (currentState == "erase")
        {
            // Highlight erase tool

            drawCircle.GetComponent<SpriteRenderer>().enabled = false; 
            eraseCircle.GetComponent<SpriteRenderer>().enabled = true;

            // Disable draw constrainer game object's collider

            drawConstrainer.GetComponent<Collider2D>().enabled = false;




            // If a line is found with the erase tool and mouse pointer, delete the line
            if (Input.GetMouseButton(0))
            {


                RaycastHit2D hit = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), .5f, Vector2.zero);
                if (hit.collider != null )
                {

                    if (hit.collider.gameObject.transform.tag == "Line")
                    {

                        Destroy(hit.collider.gameObject);

                    }

                }

            }

        }

        else if (currentState == "eraseAll")
        {

            GameObject lines = GameObject.FindWithTag("Line");

            if (lines  != null)
            {
                Destroy(lines);

            }

            // Set the tool back to draw after lines have been deleted

            if(lines == null)
            {
                currentState = "draw";

            }


        }

    }
   
    private Vector3 calculateMouseMovement()
    {
        return Input.mousePosition - lastMouseCoordinate;

    }

    private void PlayDrawAndEraseSounds()
    {
        var mouseDelta = calculateMouseMovement();

        // Set last mouse coordinate to current mouse position 
        lastMouseCoordinate = Input.mousePosition;


        // Play sound if mouse is moving and mouse button is pressed
        if (mouseDelta.magnitude > 0)
        {
            if (Input.GetMouseButton(0))
            {
                audioSource.UnPause();
            }
        }

        // Pause sound if mouse is idle
        if (mouseDelta.magnitude == 0)
        {
            audioSource.Pause();

        }


        // Pause sound after done using the tool
        if (Input.GetMouseButtonUp(0))
        {
            audioSource.Pause();

        }
    }



    void drawButtonPressed()
    {
        // load relevent audio clip
        audioSource.clip = audioClips[0];
        audioSource.Play();

        currentState = "draw";


    }

    void eraseButtonPressed()
    {
        // load relevent audio clip
        audioSource.clip = audioClips[1];
        audioSource.Play();

        currentState = "erase";

    }

    void eraseAllButtonPressed()
    {

        currentState = "eraseAll";

    }


}

