using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObstacle : MonoBehaviour
{
    public ragdollScript ragdollScript;
    public GameObject loseCanvas;
    public GameObject obstacleExcavator;
    public GameObject obstacleGarbage;
    public GameObject obstacleHydrant;
    public GameObject obstacleCrane;

    // main script
    public Main main;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void ExcavatorDie()
    {
        //Debug.Log("excavator");
        disable(obstacleExcavator);
    }


    public void HydrantDie()
    {
        //Debug.Log("hydrant");
        disable(obstacleHydrant);


    }

    public void GarbageDie()
    {
        //Debug.Log("garbage");
        disable(obstacleGarbage);


    }

    public void CraneDie()
    {
        //Debug.Log("crane");
        disable(obstacleCrane);


    }

    private void disable(GameObject canvas)
    {
        ragdollScript.ragdolEffectOnOff = true;
        StartCoroutine(main.popUp(loseCanvas));
        StartCoroutine(main.popUp(canvas));
        main.disableAll("Construction");
        main.playLoseConditionVoiceOver(main.obstacle);

    }
}
