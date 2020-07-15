using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : MonoBehaviour
{
    private PlayerObstacle wheel;

    private void Start()
    {
        wheel = GameObject.FindGameObjectWithTag("Wheel").GetComponent<PlayerObstacle>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.GetComponent<PlayerObstacle>())
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wheel"))
        {
            wheel.CraneDie();
            //Debug.Log("Crane collision detected");
            //Debug.Log("huh");

        }

    }
}
