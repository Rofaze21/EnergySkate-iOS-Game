using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class IntroGame : MonoBehaviour
{

    // Start is called before the first frame update

    public void PlayVideo()
    {
        SceneManager.LoadScene("Intro Film");
    }
    public void PlayTutorial()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
   
}