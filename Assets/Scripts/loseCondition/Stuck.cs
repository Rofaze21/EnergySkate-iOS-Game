using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class Stuck : MonoBehaviour
{

    //  trigger lose conditions: stuck

    // length
    int length;
    private float limit = 0.0f;
    private bool isMoving;
    private bool isJagged;
    Vector3[] previousPos;
    [HideInInspector] public Vector3 initPos;

    // other scripts
    public Main main;
    public Trigger trigger;
    public Win win;

    // pop up
    public GameObject winCanvas;
    public GameObject loseCanvas;
    public GameObject stuck;
    public GameObject jagged;

    // jagged stuck
    List<GameObject> collisions = new List<GameObject>();
    int startPos = 4;
    int endPos = 4;
    private Line lineScript;

    // kinetic slider
    public Slider kinetic;

    [HideInInspector] public float angle;

    public Button playButton;

    private void Start()
    {
        // fetch basic parameters

        length = main.stuck_length;
        previousPos = new Vector3[length];

        for (int i = 0; i < length; i++)
        {
            previousPos[i] = Vector3.zero;
        }
        playButton.onClick.AddListener(playButtonClicked);

    }

    void playButtonClicked()
    {
        initPos = transform.position;

    }
    void Update()
    {
        //if (Input.GetButtonDown("Play"))
        //{
        //    initPos = transform.position;
        //}


        if ((transform.position != initPos)
            && initPos != Vector3.zero)
        {
            for (int i = 0; i < length - 1; i++)
            {
                previousPos[i] = previousPos[i + 1];

            }
            previousPos[length - 1] = transform.position;

            for (int i = 0; i < length - 1; i++)
            {
                if (Vector3.Distance(previousPos[i], previousPos[i + 1]) != limit)
                {
                    isMoving = true;
                    break;
                }
                else
                {
                    isMoving = false;
                }
            }

            if (!isMoving)
            {
                //Debug.Log(initPos.ToString() + "========" + transform.position.ToString());
                if (trigger.surface)
                {
                    win.win();
                }
                else
                {
                    
                    main.playAnim("Stuck", "Stuck");
                    StartCoroutine(main.popUp(loseCanvas));
                    if (isJagged || angle <= main.jaggedAngle)
                    {
                        //Debug.Log("lose because of jagged");
                        StartCoroutine(main.popUp(jagged));
                    }
                    else
                    {
                        //Debug.Log("I'm not moving");
                        StartCoroutine(main.popUp(stuck));

                    }

                }

            }
        }
        else {
            //Debug.Log("my position" + transform.position);
            //Debug.Log("initial position" + initPos);
            //Debug.Log("position distance" + Vector3.Distance(transform.position, initPos));
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Add the GameObject collided with to the list.
        collisions.Add(collision.gameObject);
        List<Vector2> turn = new List<Vector2>();

        // Print the entire list to the console.
        for (int j = collisions.Count - 1; j >= 0; j--)
        {
            GameObject line = collisions[j];
            if (line.name != "Line_Normal(Clone)")
            {
                break;
            }
            //print(line.name);
            lineScript = line.GetComponent<Line>();
            List<Vector2> pointsList = lineScript.pointsList;

            for (int i = 0; i < pointsList.Count - 1; i++)
            {
                // turning point
                if (pointsList[i + 1].y > pointsList[i].y)
                {
                    // make sure index in range
                    if (i - startPos >= 0)
                    {
                        turn.Add(pointsList[i - startPos]);
                    }
                    else
                    {
                        turn.Add(pointsList[0]);
                    }

                    turn.Add(pointsList[i]);

                    if (i + endPos < pointsList.Count)
                    {
                        turn.Add(pointsList[i + endPos]);
                    }
                    else
                    {
                        turn.Add(pointsList[pointsList.Count - 1]);
                    }

                    Jagged(turn);
                    break;

                }
            }
        }


    }

    void Jagged(List<Vector2> turn)
    {
        angle = Vector2.Angle(turn[0] - turn[1], turn[2] - turn[1]);
        Debug.Log("angle is" + angle);
        //Debug.Log(turn[0]);
        //Debug.Log(turn[1]);
        //Debug.Log(turn[2]);
        if (angle <= main.jaggedAngle && angle > 0
            && kinetic.value <= 1)
            //&& !isMoving)
        {
            isJagged = true;
            main.playAnim("Stuck", "Stuck");
            StartCoroutine(main.popUp(loseCanvas));
            StartCoroutine(main.popUp(jagged));
        }

    }
}
