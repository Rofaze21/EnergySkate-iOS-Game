using UnityEngine;
using UnityEngine.UI;
public class Backward : MonoBehaviour
{

    //  trigger lose conditions: stuck

    private bool forward;
    [HideInInspector] public Vector3 initPos;
    Vector3[] previousPos;

    // pop ups
    public GameObject winCanvas;
    public GameObject loseCanvas;
    public GameObject stuck;
    public GameObject jagged;

    // other scripts
    public Main main;
    public Trigger trigger;
    public Stuck stuckScript;

    // length
    int length;

    public Button playButton;

    private void Start()
    {

        // fetch basic parameters

        length = main.backward_length;
        previousPos = new Vector3[length];
        for (int i = 0; i < length; i++)
        {
            previousPos[i] = Vector3.zero;
        }

        playButton.onClick.AddListener(playButtonClicked);
    }

    void  playButtonClicked()
    {
        initPos = transform.position;

    }
    void Update()
    {
      

        if ((transform.position != initPos
            && initPos != Vector3.zero))
        {
            for (int i = 0; i < length - 1; i++)
            {
                previousPos[i] = previousPos[i + 1];

            }
            previousPos[length - 1] = transform.position;

            for (int i = 0; i < length - 1; i++)
            {
                if (previousPos[i].x <= previousPos[i + 1].x)
                {
                    forward = true;
                    break;
                }
                else 
                {
                    forward = false;
                }
            }

            if (!forward && !trigger.surface)
            {
                if (stuckScript.angle > main.jaggedAngle)
                {
                    //Debug.Log("lose because of going backward");
                    StartCoroutine(main.popUp(loseCanvas));
                    StartCoroutine(main.popUp(stuck));
                    main.playAnim("Stuck", "Backward");
                    main.playLoseConditionVoiceOver(main.stuck);


                }
                else
                {
                    //Debug.Log("lose because of jagged <in backward script>");
                    StartCoroutine(main.popUp(loseCanvas));
                    StartCoroutine(main.popUp(jagged));
                    main.playAnim("Stuck", "Stuck");
                    main.playLoseConditionVoiceOver(main.jaggedStuck);

                }

            }
        }
    }

}
