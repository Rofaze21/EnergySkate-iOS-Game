using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundPrefs : MonoBehaviour
{
    public bool playerFeedbackOn = true;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void turnPlayBackSoundOff()
    {
        playerFeedbackOn = false;
        //main.turnLoseConiditionSoundOff = true;
    }
    public void turnPlayBackSoundOn()
    {
        playerFeedbackOn = true;
        //main.turnLoseConiditionSoundOff = false;
    }

}
