using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class skateSound : MonoBehaviour
{

    public AudioSource audioSource;
    public Slider KinecticEnergy;
    private int min = 0;
    private int max = 150;
    public bool playModeActivated = false;
    public bool audioPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
            //audioSource.Pause();

    }
   
    // Update is called once per frame
    void Update()
    {
        if(playModeActivated == true) {
            if(audioPlayed == false) { 
            audioSource.Play();
            audioPlayed = true;
            }

        var normalizedFloat = (KinecticEnergy.value - min) / (max - min);

        if (normalizedFloat == 0)//Not Moving
         {
              audioSource.volume = 0f;

         }
            if (normalizedFloat > .05) //Moving a little faster, set dynamic volume
        {

            audioSource.volume = normalizedFloat;

        }
        else
        {
            audioSource.volume = .05f;
        }
        } else
        {
            audioSource.volume = 0f;

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Line_Normal(Clone)")
        {
          
            audioSource.UnPause();

            //Debug.Log("touching Line");
        } else
        {
            audioSource.Pause();
        }
    }
}
