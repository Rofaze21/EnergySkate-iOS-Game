using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickSounds : MonoBehaviour
{
    public Button drawButton;
    public Button eraseButton;
    public Button eraseAllButton;
    public Button playButton;
    //public Button pauseButton;

    public AudioClip clickSound;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        drawButton.onClick.AddListener(drawButtonPressed);
        eraseButton.onClick.AddListener(eraseButtonPressed);
        eraseAllButton.onClick.AddListener(eraseAllButtonPressed);
        playButton.onClick.AddListener(playButtonPressed);
        //pauseButton.onClick.AddListener(pauseButtonPressed);

    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
    
    void drawButtonPressed()
    {
        audioSource.PlayOneShot(clickSound);

    }

    void eraseButtonPressed()
    {

        audioSource.PlayOneShot(clickSound);

    }

    void eraseAllButtonPressed()
    {
        audioSource.PlayOneShot(clickSound);


    }
    void playButtonPressed()
    {

        audioSource.PlayOneShot(clickSound);

    }

    void pauseButtonPressed()
    {
        audioSource.PlayOneShot(clickSound);

    }
}
