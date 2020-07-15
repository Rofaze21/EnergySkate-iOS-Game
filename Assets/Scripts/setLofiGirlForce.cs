using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;


public class setLofiGirlForce : MonoBehaviour
{
    
    public Slider speedSlider;

    public TextMeshPro forceText;

    public Main mainScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        forceText.text = Mathf.RoundToInt(speedSlider.value).ToString();
    }
}
