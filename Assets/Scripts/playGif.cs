using UnityEngine;
using UnityEngine.UI;
using OldMoatGames;
using System.Collections;
using UnityEngine.SceneManagement;
public class playGif : MonoBehaviour
{
    private AnimatedGifPlayer AnimatedGifPlayer;
    public AudioSource audioSouce;
    private bool startVideo = false;

    public void Awake()
    {
        // Get the GIF player component
        AnimatedGifPlayer = GetComponent<AnimatedGifPlayer>();

        // Set the file to use. File has to be in StreamingAssets folder or a remote url (For example: http://www.example.com/example.gif).
        AnimatedGifPlayer.FileName = "video.gif";

        // Disable autoplay
        AnimatedGifPlayer.AutoPlay = false;

        //// Add ready event to start play when GIF is ready to play
        AnimatedGifPlayer.OnReady += OnGifLoaded;

        // Add ready event for when loading has failed
        AnimatedGifPlayer.OnLoadError += OnGifLoadError;

        // Init the GIF player
        AnimatedGifPlayer.Init();
        //Play();

    }

    private void Update()
    {
        if (startVideo == true)
        {
            if (AnimatedGifPlayer.State == GifPlayerState.Stopped)
            {
                AnimatedGifPlayer.TimeScale = 0;
                Debug.Log("Done Playing Video");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    IEnumerator skipVideoAfterTime() {

        yield return new WaitForSeconds(21f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }




    private void OnGifLoaded()
    {
        AnimatedGifPlayer.Play();
        audioSouce.Play();
        startVideo = true;
        AnimatedGifPlayer.TimeScale = 1;
        StartCoroutine("skipVideoAfterTime");
        Debug.Log(AnimatedGifPlayer.State + "during onGifLoaded");
        Debug.Log("GIF size: width: " + AnimatedGifPlayer.Width + "px, height: " + AnimatedGifPlayer.Height + " px");
    }

    private void OnGifLoadError()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        Debug.Log("Error Loading GIF");
    }

    public void Play()
    {
        // Start playing the GIF
        Debug.Log(AnimatedGifPlayer.State + "during Play");


    }


    private IEnumerator play()
    {

        yield return new WaitForSeconds(1);
        audioSouce.Play();
        AnimatedGifPlayer.Play();
        startVideo = true;

    }


    public void OnDisable()
    {
        AnimatedGifPlayer.OnReady -= Play;
    }
}