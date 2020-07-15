using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class CursorChangingScript : MonoBehaviour
{

    //Get Draw Canvases Graphic raycaster
    public GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    // Srite Renderer and sprites to switch between
    public SpriteRenderer spriteRenderer;
    public Sprite pointer;
    public Sprite draw;
    public Sprite erase;

    public LineCreator lineCreator;

    public float yDirection;
    public float xDirection;

   
    bool keepPointer = false;
    void Start()
    {
        Cursor.visible = false;

        
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // New position of cursor sprite because it needs to be adjusted a little bit
        mousePos.y = mousePos.y - yDirection;
        mousePos.x = mousePos.x - xDirection;

        // pointer cursor follows pointer
        transform.position = mousePos;

        if (keepPointer == false)
        {
            

            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();
            results.Clear();
            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray

            // when user has hovered over a UI element(layer 5) then configure pointer cursor 
            if (results.Count == 1 && results[results.Count - 1].gameObject.layer == 5)
            {
                transform.localScale = new Vector3(.3f, .3f, 0f);


                yDirection = 0.55f;
                xDirection = -0.8f;

                //spriteRenderer.sprite = pointer;

                spriteRenderer.sprite = null;


            }

            // when user is not touchig UI elements, configure relevent cursor
            if (results.Count == 0)
            {

                transform.position = mousePos;

                if (lineCreator.currentState == "draw")
                {
                    transform.localScale = new Vector3(1f, 1f, 0f);

                    yDirection = 1.55f;
                    xDirection = -1.1f;

                    spriteRenderer.sprite = draw;
                }
                if (lineCreator.currentState == "erase")
                {
                    transform.localScale = new Vector3(1f, 1f, 0f);

                    yDirection = 1.1f;
                    xDirection = -.4f;

                    spriteRenderer.sprite = erase;
                }
            }

        }
    }

    public void changeToPointer()
    {
        keepPointer = true;
        spriteRenderer.sprite = null;
        //spriteRenderer.sprite = null;
    }

    public void changeToNormal()
    {
        keepPointer = false;
        //renderer.sprite = null;

    }

}
