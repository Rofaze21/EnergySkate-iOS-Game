using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableSkip : MonoBehaviour
{

    public GameObject skipButton;

    // Start is called before the first frame update
    void Start()
    {
        
          StartCoroutine("showSkipButton");

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     IEnumerator showSkipButton()
    {
        skipButton.SetActive(false);

        yield return new WaitForSeconds(4f);
        skipButton.SetActive(true);
        
    }
}
