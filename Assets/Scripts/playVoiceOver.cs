using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playVoiceOver : MonoBehaviour
{
    public AudioSource audioSource;

    private bool audioIsPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Make sure that voice over audio plays becasue of webgl bugs
        if(audioIsPlaying == false)
        {
            audioSource.Play();
            audioIsPlaying = true;
        }
    }
}
