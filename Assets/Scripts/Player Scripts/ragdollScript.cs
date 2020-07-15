using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdollScript : MonoBehaviour
{

    // list of individual body parts of lofi girl
    public List<GameObject> gameobjectsList;

    
    [HideInInspector] public Animator anim;

    //lofi Girl's collider
    public Collider2D colliders;


    // Controls the ragdoll effect on lofi girl
    public bool ragdolEffectOnOff;


    // Scripts on body parts to disable for ragdoll effect to show
    public List<MonoBehaviour> scriptList;

    //For bone cracking sounds
    AudioSource audioSource;

    // List of different bone cracking sounds
    public List<AudioClip> boneCrackingSounds;

    bool boneCrackingSoundPlayed = false;

    // controls how many bone cracking sounds should be played after colliding with object
    public int numberOfBoneCrackingSounds = 4;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ragdollEffect(ragdolEffectOnOff);

    }

    public void ragdollEffect(bool s)
    {

        //When ragdoll effect is true, then play bone cracking sounds  
        if (s == true) 
        {
            if (boneCrackingSoundPlayed == false && audioSource.isPlaying == false) 
            {
                var randomInt = Random.Range(0f, (float)boneCrackingSounds.Count  - 1f); // Pick a random int

                audioSource.clip = boneCrackingSounds[(int)randomInt]; // load up bone cracking sound

                audioSource.Play(); 

               
                numberOfBoneCrackingSounds--; // decrement number of bones
                


                
                if(numberOfBoneCrackingSounds <= 0) // stop playing bone cracking sounds once boneCrackingSounds is equal to 0
                {
                    boneCrackingSoundPlayed = true;
                }


            }
        }

        // Enable and disable relevent scripts and game objects for ragdoll effect to take effect
        for (int i = 0; i < gameobjectsList.Count; i++)
        {
            gameobjectsList[i].GetComponent<Collider2D>().enabled = s;

            //if gameobject has a HingeJoint2D, then enable it
            if (gameobjectsList[i].TryGetComponent(out HingeJoint2D hinge) == true)
            {
                hinge.enabled = s;
            }


            if(s == false)
            {
                gameobjectsList[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
               

            } else
            {

                gameobjectsList[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            }


            anim.enabled = !s;
            colliders.enabled = !s;

        }

        // Disable/Enable relevent scripts
        for (int i = 0; i < scriptList.Count; i++)
        { 
            scriptList[i].enabled = !s;
        }

    }
}
