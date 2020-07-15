using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Should be renamed to level manager
public class ReplayGame : MonoBehaviour
{
    public Main mainScript;
    public void PlayGame()
    {
        var currentScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void nextLevel()
    {
        var currentScene = SceneManager.GetActiveScene();
        DestoryLines();
        SceneManager.LoadScene(currentScene.buildIndex + 1);

    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }

    public void DestoryLines()
    {
        mainScript.destroyLines();
    }
}
