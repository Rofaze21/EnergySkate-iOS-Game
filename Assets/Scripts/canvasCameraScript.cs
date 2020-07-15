using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasCameraScript : MonoBehaviour
{
    private Canvas otherCanvas;
    public Canvas canvas;
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        otherCanvas = FindObjectOfType<Canvas>();

        canvas.worldCamera = otherCanvas.worldCamera;

    }

    // Update is called once per frame
    void Update()
    {
        if(canvas.worldCamera == null)
        {
            canvas.worldCamera = otherCanvas.worldCamera;

            canvas.worldCamera = mainCamera;

        }
    }
}
