using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Lean.Gui;
using System.Collections.Generic;
// provide basic utils and constants params
public class Main : MonoBehaviour
{
    // wait x secs before pop ups
    public int pop_up = 2;

    // cut off kinetic energy for win
    public int CUT_OFF = 50;

    public float jaggedAngle = 30.0f;

    public Rigidbody2D wheelRB;

    // length
    public int backward_length = 10;
    public int stuck_length = 20;

    // ac
    public Animator anim;

    public Camera playerCamera;
    public Camera mainCamera;

    // lose canvas
    public GameObject loseCanvas;

    // constructions
    public GameObject excavator;
    public GameObject hydrant;
    public GameObject crane;
    public GameObject garbage;

    [HideInInspector] public bool move;

    [HideInInspector] public float speed = 2.0f;
    [HideInInspector] public Vector3 initPos;
    Vector3 finalPos;

    // buttons
    public Button restartButton;
    public LeanButton nextLevel;

    // scroll bar automatic scrolling
    public Scrollbar scrollbar;
    public float scrollSpeed = 0.8f;
    [HideInInspector] public bool toScroll;
    bool back;
    bool forth;
    public int SCROLL_TO = 1;
    bool pause = false;
    public int SCROLL_BACK = 3;

    public float scrollBarMaxValue = 1.0f;

    // scroll only in initial start
    [HideInInspector] public bool initialStart;

    public ragdollScript ragdollScript;

    public GameObject lineCreator;
    // Start is called before the first frame update

    //sound
    public AudioSource playerAudio;
    public AudioClip kickkick;
    public AudioClip skate;


    //sounds for loseConditions
    public AudioClip buldingCrash;
    public AudioClip Crash;
    public AudioClip fell;
    public AudioClip jaggedStuck;
    public AudioClip obstacle;
    public AudioClip stuck;
    public AudioClip win;



    //AudioSource for lose conditions
    public AudioSource loseConiditionsAudioSource;

    //Bool to make loseCondition Play only Once
    private bool loseConiditionPlayed = false;

    //play loseConiditions or not
    public bool turnLoseConiditionSoundOff = false;

    private bool kickedOff = false;

    // replayBtn object
    public GameObject restartBtn;

    // force to add with initial kick 
    public float Force = 2f;

    
    
    void Start()
    {
        

        // record what level is currently playing rn
        var currentScene = SceneManager.GetActiveScene();
        string currentID = currentScene.buildIndex.ToString();

        // if currentID not registered yet
        // meaning: a new level
        // destroy all prev lines
        // register currentID with a value == 1
        if (!PlayerPrefs.HasKey(currentID))
        {
            destroyLines();
            PlayerPrefs.SetInt(currentID, 1);
        }

        //// if currentID already registered in pref
        //// and has a value of 1
        //if (PlayerPrefs.HasKey(currentID)
        //    && PlayerPrefs.GetInt(currentID) == 1)
        //{

        //}
        ragdollScript.ragdolEffectOnOff = false;


        initPos = transform.position;
        finalPos = initPos + new Vector3(5, 0, 0);
        move = false;

        // automatic scroll
        toScroll = true;
        forth = true;

        // listeners for btns
        restartButton.onClick.AddListener(restartOnClick);
        nextLevel.OnClick.AddListener(nextOnClick);
        // initial start is true
        initialStart = true;

    }

    private void nextOnClick()
    {
        PlayerPrefs.DeleteKey("initialStart");
    }

    public void destroyLines()
    {
        GameObject[] lines = GameObject.FindGameObjectsWithTag("Line");
        foreach (GameObject line in lines)
        {
            Destroy(line);
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (toScroll && initialStart)
        {
            if (!PlayerPrefs.HasKey("initialStart"))
            {
                //Debug.Log("first time playing " + PlayerPrefs.GetInt("initialStart"));
                StartCoroutine(scroll());
            }
        }

        //if (Input.GetButtonDown("Play"))
        //{
        //    lineCreator.SetActive(false); //Disable LineCreator Script

        //    move = true;


        //}
        toMove();

    }


    public void restartOnClick()
    {
        var scene = SceneManager.GetActiveScene().buildIndex;

        ragdollScript.ragdolEffectOnOff = false;

        initialStart = false;
        PlayerPrefs.SetInt("initialStart", 0);
        SceneManager.LoadScene(scene);
        lineCreator.SetActive(true);
    }

    public void toMove()
    {
        if (move == true && kickedOff == false)
        {
            //transform.Translate(Vector2.right * speed * Time.deltaTime);
            //StartCoroutine("KickOff");

            //wheelRB.AddForce(new Vector2(8f, 0));
            //kickedOff = true;

        }

        if (move)
        {
            //Debug.Log("adding normal force");
            wheelRB.AddForce(new Vector2(Force, 0));

        }
        if (transform.position.x >= finalPos.x)
        {
            move = false;
        }

        //if (move)
        //{
        //    wheelRB.AddForce(new Vector2(0.2f, 0));
        //}


    }

    public void kickedOffBool()
    {
        kickedOff = false;
    }

    IEnumerator KickOff()
    {
        //playerAudio.PlayOneShot(skate);

        wheelRB.AddForce(new Vector2(4f, 0));
        Debug.Log("Adding special beginigng force");

        yield return new WaitForSeconds(2f);

        
    }

    // automatic scroll
    public IEnumerator scroll()
    {
        if (toScroll)
        {
            

            if (forth && scrollbar.value < scrollBarMaxValue)
            {
                lineCreator.SetActive(false);

                if (scrollbar.value == 0.0f)
                {
                    yield return new WaitForSeconds(SCROLL_TO);
                }
                scrollbar.value += (scrollSpeed * Time.deltaTime);

            }
            if (scrollbar.value >= scrollBarMaxValue)
            {
                if (!pause)
                {
                    yield return new WaitForSeconds(SCROLL_BACK);
                    pause = true;
                }
                back = true;
                forth = false;
            }
            if (back && !forth)
            {
                //Debug.Log("scrollback" + scrollbar.value);
                scrollbar.value -= (scrollSpeed * Time.deltaTime);
            }

            if (scrollbar.value <= 0)
            {
                lineCreator.SetActive(true);
                back = false;
                forth = false;
                toScroll = false;
                initialStart = false;
                PlayerPrefs.SetInt("initialStart", 1);
            }
        }
    }

    //private void OnApplicationQuit()
    //{
    //    PlayerPrefs.DeleteKey("initialStart");
    //    PlayerPrefs.DeleteKey("1");
    //    PlayerPrefs.DeleteKey("2");
    //    PlayerPrefs.DeleteKey("3");
    //    PlayerPrefs.DeleteKey("4");
    //    PlayerPrefs.DeleteKey("5");
    //}

    public void playAnim(
        string animation, string condition)
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Stuck")
                && !anim.GetCurrentAnimatorStateInfo(0).IsName("Crash")
                && !anim.GetCurrentAnimatorStateInfo(0).IsName("Excavator")
                && !anim.GetCurrentAnimatorStateInfo(0).IsName("Win")
                && !anim.GetCurrentAnimatorStateInfo(0).IsName("Garbage")
                && !anim.GetCurrentAnimatorStateInfo(0).IsName("Hydrant")
                && !anim.GetCurrentAnimatorStateInfo(0).IsName("Crane"))
        {
            //Debug.Log(anim + " anim");
            anim.SetTrigger(animation);
            //ragdollScript.ragdollEffect(true);

        }
        disableAll(condition);
        //StartCoroutine(restart());
    }

    public void disableAll(string condition)
    {
        if (condition.Equals("Trigger"))
        {
            GetComponent<Stuck>().enabled = false;
            GetComponent<Backward>().enabled = false;
            GetComponent<Win>().enabled = false;
            disableConstruction();
        }
        else if (condition.Equals("Stuck"))
        {
            //Debug.Log("only stuck");
            GetComponent<Trigger>().enabled = false;
            GetComponent<Backward>().enabled = false;
            GetComponent<Win>().enabled = false;
            disableConstruction();
            //playLoseConditionVoiceOver(stuck);

        }
        else if (condition.Equals("Backward"))
        {
            GetComponent<Stuck>().enabled = false;
            GetComponent<Trigger>().enabled = false;
            GetComponent<Win>().enabled = false;
            disableConstruction();
            //playLoseConditionVoiceOver(stuck);

        }
        else if (condition.Equals("Construction"))
        {
            GetComponent<Stuck>().enabled = false;
            GetComponent<Trigger>().enabled = false;
            GetComponent<Backward>().enabled = false;
            GetComponent<Win>().enabled = false;
            //playLoseConditionVoiceOver(obstacle);

        }
        else if (condition.Equals("Win"))
        {
            GetComponent<Stuck>().enabled = false;
            GetComponent<Trigger>().enabled = false;
            GetComponent<Backward>().enabled = false;
            //playLoseConditionVoiceOver(Crash);
            disableConstruction();
        }
    }

    public bool hasPopup(bool lost)
    {
        for (int i = 0; i < loseCanvas.gameObject.transform.childCount; i++)
        {
            Transform losePopup = loseCanvas.gameObject.transform.GetChild(i);
            if (losePopup.gameObject.activeSelf)
            {
                lost = true;
                break;
            }

        }
        return lost;
    }

    private void disableConstruction()
    {
        excavator.SetActive(false);
        garbage.SetActive(false);
        crane.SetActive(false);
        hydrant.SetActive(false);
    }

    public IEnumerator popUp(GameObject canvas)
    {
        yield return new WaitForSeconds(pop_up);
        
        if (!hasPopup(false))
        {
            restartBtn.SetActive(false);
            canvas.SetActive(true);
        }
    }

    public void playLoseConditionVoiceOver(AudioClip clip)
    {

        Debug.Log(clip + " playing that clip");
        if(turnLoseConiditionSoundOff == false) { //Check to see if Toggle is On
            if (loseConiditionPlayed == false)
            {
                loseConiditionsAudioSource.clip = clip;
                loseConiditionsAudioSource.Play();
                loseConiditionPlayed = true;

            }
        }
    }
}
