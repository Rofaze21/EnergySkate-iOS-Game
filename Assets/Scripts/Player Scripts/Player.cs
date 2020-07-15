using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Player : MonoBehaviour
{
    public Rigidbody2D lofiRB;
    public GameObject lofigirl;
    public Camera cam1;
    public Camera cam2;
    public Slider potentialEnergy;
    public Slider kinecticEnergy;

    public bool kickedOff;

    // ac
    [HideInInspector] public Animator anim;

    
    // to get .y 
    public GameObject ground;

    // Main script
    public Main main;
    public Stuck stuck;
    public Backward backward;


    public GameObject lineCreator;

    public GameObject playerCanvas;

    private void Start()
    {

        //Turn player camera on and main camera off
        cam1.enabled = true;
        cam2.enabled = false;
        anim = lofigirl.GetComponent<Animator>();
    }


    void Update()
    {
        
       

        // move the character frame by frame
        // will need the .deltaTime, since every frame takes up
        // different periods of time
        

        //var velocity = lofiRB.velocity.y;
        double mass = lofiRB.mass;
        // normalize velocity
        double velocity = lofiRB.velocity.magnitude;
        double kinetic = 0.5 * mass
            * velocity * velocity;
        //Debug.Log(kinetic);
        double potential = mass * 10 * (lofigirl.transform.position.y -
            ground.transform.position.y);

        //Debug.Log(potential);
        kinecticEnergy.value = (float) kinetic;
        potentialEnergy.value = (float) potential;

        //Debug.Log(kinecticEnergy.value);
        //Debug.Log("potential" + potentialEnergy.value);
        //Debug.Log("============");
        //Debug.Log(potentialEnergy.maxValue);


    }
   

    

    
  
}
