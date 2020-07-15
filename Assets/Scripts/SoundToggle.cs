using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Gui;
public class SoundToggle : MonoBehaviour
{
    public Main main;
    private GameObject musicPrefs;
    public LeanToggle playerFeedbackToggle;
    private bool toggleChecked = false;
    private void Awake()
    {
        //SoundPrefs = FindObjectOfType<soundControl>();
        //playerFeedbackOn = SoundPrefs.playerFeedbackOn;
        //playerFeedbackToggle.On = playerFeedbackOn;
    }
    // Start is called before the first frame update
    void Start()
    {
        //playerFeedbackOn = SoundPrefs.playerFeedbackOn;
        //playerFeedbackToggle.On = playerFeedbackOn;


        musicPrefs = GameObject.FindGameObjectWithTag("music");
            //.GetComponent<AudioSource>().volume = musicSlider.value;


        //SoundPrefs = FindObjectOfType<soundControl>();
        //playerFeedbackOn = SoundPrefs.playerFeedbackOn;
        //playerFeedbackToggle.On = playerFeedbackOn;
    }

    // Update is called once per frame
    void Update()
    {
        if (toggleChecked == false)
        {
            if (musicPrefs != null)
            {

                playerFeedbackToggle.On = musicPrefs.GetComponent<MusicDontDestroy>().playerFeedbackOn;
                main.turnLoseConiditionSoundOff = !musicPrefs.GetComponent<MusicDontDestroy>().playerFeedbackOn;
                toggleChecked = true;
            }

        }
       
    }

    public void turnPlayBackSoundOff()
    {
        musicPrefs.GetComponent<MusicDontDestroy>().playerFeedbackOn = false;
        main.turnLoseConiditionSoundOff = true;

    }
    public void turnPlayBackSoundOn()
    {
        musicPrefs.GetComponent<MusicDontDestroy>().playerFeedbackOn = true;
        main.turnLoseConiditionSoundOff = false;

    }
}
