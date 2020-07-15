using UnityEngine;
using UnityEngine.UI;
using OldMoatGames;
using System.Collections;
using UnityEngine.SceneManagement;
public class CodeExample : MonoBehaviour {
    private AnimatedGifPlayer AnimatedGifPlayer;
    public AudioSource audioSouce;
    private bool startVideo = false;

    public void Awake() {
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
        if(startVideo == true) { 
        if (AnimatedGifPlayer.State != GifPlayerState.Playing)
        {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        }
    }
    private void Start()
    {
        StartCoroutine("play");
    

    }
    private void OnGifLoaded()
    {
        //PlayButton.interactable = true;

        Debug.Log("GIF size: width: " + AnimatedGifPlayer.Width + "px, height: " + AnimatedGifPlayer.Height + " px");
    }

    private void OnGifLoadError() {
        Debug.Log("Error Loading GIF");
    }

    public void Play() {
        // Start playing the GIF
        AnimatedGifPlayer.Play();

     
    }

   
     private IEnumerator play()
    {
        
            yield return new WaitForSeconds(2);
            audioSouce.Play();
            AnimatedGifPlayer.Play();
            startVideo = true;


    }


    public void OnDisable() {
        AnimatedGifPlayer.OnReady -= Play;
    }


}
