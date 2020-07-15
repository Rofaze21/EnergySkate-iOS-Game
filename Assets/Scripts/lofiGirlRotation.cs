using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lofiGirlRotation : MonoBehaviour
{

    public GameObject lofiGirl;

    public Vector3 rotation;

    public Collider2D leftWheel;
    public Collider2D rightWheel;



    // Start is called before the first frame update
    void Start()
    {
        rotation = lofiGirl.transform.rotation.eulerAngles;


    }
    // Update is called once per frame
    void Update()
    {
        rotation.z += 1.0f;
        lofiGirl.transform.rotation = Quaternion.Euler(rotation);

        leftWheel.IsTouchingLayers();
    }
}
