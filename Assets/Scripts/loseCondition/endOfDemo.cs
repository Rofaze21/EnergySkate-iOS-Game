using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endOfDemo : MonoBehaviour
{
    // endOfDemo btn
    public Button endOfDemoBtn;

    // main script
    public Main main;

    // Start is called before the first frame update
    void Start()
    {
        endOfDemoBtn.onClick.AddListener(demoClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void demoClick()
    {
        main.destroyLines();

    }
}
