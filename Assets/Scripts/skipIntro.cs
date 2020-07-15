using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class skipIntro : MonoBehaviour
{
    public Button skipButton;
    

    void Start()
    {
        skipButton.onClick.AddListener(skipButtonPressed);
    }

    void skipButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
