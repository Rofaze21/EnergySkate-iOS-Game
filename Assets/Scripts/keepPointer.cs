using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepPointer : MonoBehaviour
{

    // When in play mode or screen other than levels, keep the pointer cursor on


    public SpriteRenderer spriteRenderer;
    public Sprite pointer;
    public Camera playerCamera;

    public float yDirection;
    public float xDirection;

    public Vector3 scale;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        spriteRenderer.sprite = pointer;
        transform.localScale = scale;

        if(yDirection == 0 && xDirection == 0) { 
        yDirection = 0.15f;
        xDirection = .4f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // pointer cursor follows pointer
        Vector2 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);

    
        mousePos.x = mousePos.x - xDirection;
        mousePos.y = mousePos.y - yDirection;

        transform.position = mousePos;
    }

   
}
