using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Trigger : MonoBehaviour
{
    //  trigger lose conditions: buildings and road
    public Collider2D building1;
    public BoxCollider2D building2Lose;
    public EdgeCollider2D building2Win;
    public Collider2D road;

    public CapsuleCollider2D chimney;
    public Slider kinetic;

    public Button restartButton;

    // win flag
    [HideInInspector] public bool surface = false;

    // other scripts
    public Main main;
    public Win win;

    // pop up
    public GameObject winCanvas;
    public GameObject loseCanvas;
    public GameObject crash;
    public GameObject buildingStuck;
    public GameObject fell;

    // Start is called before the first frame update
    void Start()
    {
        // fetch basic parameters
        restartButton.onClick.AddListener(restartButtonPressed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision == building2Win)
        {
            // turn flag to true when hit the surface
            surface = true;

            //Debug.Log("touched surface");
            if (kinetic.value <= 1.0f)
            {
                //Debug.Log("You win because you touched surface even without " +
                //    "touching the chimney just yet");

                win.win();
            }
        }
        else if (collision == chimney)
        {
            // if <= cur off, then win
            //Debug.Log("hit when kinetic is: " + kinetic.value);
            //Debug.Log("Yout Cut Off energy is" + kinetic.value);

            if (kinetic.value <= main.CUT_OFF)
            {
                Debug.Log("You Win!");
                win.win();
                main.playLoseConditionVoiceOver(main.win);


            }
            // otherwise, lost because of too much kinetic
            else
            {
                //Debug.Log("kinetic energy is too high");
                StartCoroutine(main.popUp(loseCanvas));
                StartCoroutine(main.popUp(crash));
                main.playAnim("Crash", "Trigger");
                main.playLoseConditionVoiceOver(main.Crash);
            }
        }

        // if hit building2 sides, or road,
        // play stuck anim
        else if (collision == building2Lose)
        {
            //debugCollision(collision);
            StartCoroutine(main.popUp(loseCanvas));
            StartCoroutine(main.popUp(buildingStuck));
            main.playAnim("BuildingStuck", "Trigger");
            main.playLoseConditionVoiceOver(main.buldingCrash);

        }
        else if (collision == road)
        {
            //debugCollision(collision);
            //Debug.Log("road");
            StartCoroutine(main.popUp(loseCanvas));
            StartCoroutine(main.popUp(fell));
            main.playAnim("Fell", "Trigger");
            main.playLoseConditionVoiceOver(main.fell);
        }

    }

    void debugCollision(Collider2D collision)
    {
        Debug.Log("lose because of " + collision.name);
    }

    void restartButtonPressed()
    {
        var scene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(scene);
    }
}
