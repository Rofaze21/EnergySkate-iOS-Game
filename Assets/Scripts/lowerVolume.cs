using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class lowerVolume : MonoBehaviour
{
    public Button startButton;

    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(startButtonPressed);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void turnMusicVolumeDown()
    {
        audioSource.volume = .1f;

    }
    void startButtonPressed()
    {
        turnMusicVolumeDown();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
