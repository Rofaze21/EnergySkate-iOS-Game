using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBackgroundMove : MonoBehaviour
{
    private float speed = -.003f;


    // Start is called before the first frame update
    void Start()
    {
        //startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = new Vector2(Time.time * speed, 0);
        //Renderer.material.mainTextureOffset = offset;
        GetComponent<Renderer>().material.mainTextureOffset = offset;
        //GetComponent <SpriteRenderer>().sprite.mainTextureOffset = offset;
    }
}
