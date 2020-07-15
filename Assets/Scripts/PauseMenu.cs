using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseButton, pauseMenu;

    public Slider musicSlider;
    public Slider SFXSlider;

    public void Start()
    {
        UnPauseGame();
        musicSlider.value = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>().volume;
    }
    
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
     
    }
    public void UnPauseGame()
    {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1; 
    }
    public void QuitGame()
    {
        SceneManager.LoadScene("Opening Scene");
    }

    public void ControlMusicVolume()
    {
        GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>().volume = musicSlider.value;

    }
    public void ControlSFXVolume()
    {
        GameObject.FindGameObjectWithTag("sfx").GetComponent<AudioSource>().volume = SFXSlider.value;

    }

    public void MuteGame()
    {
       GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>().mute = true;

    }

    public void UnMuteGame()
    {
        GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>().mute = false;

    }

}  
