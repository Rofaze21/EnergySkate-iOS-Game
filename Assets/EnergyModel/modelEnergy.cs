using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class modelEnergy : MonoBehaviour
{
    //Make Potential Energy go up and down accroding to lofi girl height


    public static Dictionary<GameObject, bool> PotentialEnergyDict;
    public static Dictionary<GameObject, bool> KinecticEnergyDict;

  

    //  get each rectangle gameobject in a list
    public List<GameObject> PotentialEnergy;
    public List<GameObject> KinecticEnergy;

    //  public Material blueMaterial;

    public Material blueMaterial;
    public Material greenMaterial;
    public Material greyMaterial;

    //Animator Controller
    public Animator PotentialAnimator;
    public Animator KinecticAnimator;

    public GameObject potentialAnimatorObject;
    public GameObject kinecticAnimatorObject;


    public Slider potentialEnergy;
    public Slider kinecticEnergy;


    private int prevKinecticEnergy;
    private int prevPotentialEnergy;


  

    // Update is called once per frame
    void Update()
    {

        //Convert slider values into int
        var potentialEnergyValueFromSlider = Mathf.RoundToInt(potentialEnergy.value / 25);
        var kinecticEnergyValueFromSlider = Mathf.RoundToInt(kinecticEnergy.value / 20);

        //As soon as the animator controllers have gone through 50% of their animation time, change the color of the objects.
        if (PotentialAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f >= .5f)
        {
            potentialAnimatorObject.GetComponent<MeshRenderer>().material = greenMaterial;
        } else
        {
            potentialAnimatorObject.GetComponent<MeshRenderer>().material = blueMaterial;

        }

        if (KinecticAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f >= .5f)
        {
            kinecticAnimatorObject.GetComponent<MeshRenderer>().material = blueMaterial;
        } else
        {
            kinecticAnimatorObject.GetComponent<MeshRenderer>().material = greenMaterial;

        }


        if (potentialEnergyValueFromSlider > 0) //We only want to show transfer of energy when potential energy is positive
        {
            if (kinecticEnergyValueFromSlider > prevKinecticEnergy) // Only trigger animation if kinectic energy is higher then it previously was
            {
                if (potentialAnimatorObject.activeSelf == false) //Set the object to true if it already is not. This prevents initial animation firing
                {
                    potentialAnimatorObject.SetActive(true);

                }
                if (PotentialAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > .8f )
                {

                    PotentialAnimator.speed = speedSetter(kinecticEnergyValueFromSlider);// Set speed of animation

                   
                    PotentialAnimator.Play("energyAnimation", -1, 0f); //Play the animation

                }

            }
        }

        if (kinecticEnergyValueFromSlider > 0)//We only want to show transfer of energy when kinetic energy is positive
        {
            if (potentialEnergyValueFromSlider > prevPotentialEnergy) // Only trigger animation if potential energy is higher then it previously was
            {
             
                if (kinecticAnimatorObject.activeSelf == false) //Set the object to true if it already is not. This prevents initial animation firing
                {
                    kinecticAnimatorObject.SetActive(true);

                }
                if (KinecticAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >.8f)
                {
                    KinecticAnimator.speed =  speedSetter(kinecticEnergyValueFromSlider); //Set speed of animation


                    
                    KinecticAnimator.Play("kinectToPotential", -1, 0f); //Play Animation

                }

            }
        }

        prevKinecticEnergy = kinecticEnergyValueFromSlider; //keep track of the previous kinetic Energy variable
        prevPotentialEnergy = potentialEnergyValueFromSlider;//keep track of the previous kinetic Energy variable


        //keep track of how many energy components are filled in 
        var potentialCount = 0;
        var kinecticCount = 0;


        PotentialEnergyDict = new Dictionary<GameObject, bool>();
        KinecticEnergyDict = new Dictionary<GameObject, bool>();

        //Color game objects in potential energy, depdending upon how high the energy is
        foreach (GameObject i in PotentialEnergy)
         {
                    if (potentialCount < potentialEnergyValueFromSlider)
                    {
                         PotentialEnergyDict.Add(i, true);
                         potentialCount++;
                    } else
                    {
                        PotentialEnergyDict.Add(i, false);

                    }
                   


                }

        //Color game objects in kinetic energy, depdending upon how high the energy is

        foreach (GameObject i in KinecticEnergy)
                {

                if (kinecticCount < kinecticEnergyValueFromSlider)
                {
                    KinecticEnergyDict.Add(i, true);
                    kinecticCount++;
                }
                else
                {
                    KinecticEnergyDict.Add(i, false);

                }

                }


        //If an object is in potential energy list is active, make  it greeen, otherwise grey

        foreach (GameObject i in PotentialEnergy)
            {
                if (PotentialEnergyDict[i] == true)
                {
                    i.gameObject.GetComponent<MeshRenderer>().material = blueMaterial;

                }
                else
                {

                    i.gameObject.GetComponent<MeshRenderer>().material = greyMaterial;


                }

            }

            //If an object is in kinectic energy list is active, make  it greeen, otherwise grey
            foreach (GameObject i in KinecticEnergy)
            {
                if (KinecticEnergyDict[i] == true)
                {
                    i.gameObject.GetComponent<MeshRenderer>().material = greenMaterial;

                }
                else
                {

                    i.gameObject.GetComponent<MeshRenderer>().material = greyMaterial;


                }

            }

        
    }



    //Sets speed of the animation
    private float speedSetter(int value)
    {
        float speed = .5f;

        if (value > 7)
        {
            speed = 3f;
        }
        if (value > 5 && value <= 7)
        {
            speed = 2f;
        }
        if (value > 3 && value <= 5)
        {
            speed = 1.5f;
        }
        if (value > 2 && value <= 3)
        {
            speed = 1f;
        }
        if (value > 0 && value < 2)
        {
            speed = .5f;
        }


        return 2f;
    }

    public void addToKinecticEnergy()
    {


        // Find last true value of Potential Energy
        //Set to false

        var trueIndexOfPotential = FindLastTrueValue(PotentialEnergyDict);

        if (trueIndexOfPotential != null)
        {
            setValueToBool(trueIndexOfPotential, PotentialEnergyDict, false);
            PotentialAnimator.Play("energyAnimation", -1, 0f);
            
            //Find First False value of Kinectic Eneregy
            var falseIndexOfKinectic = findFirstFalseValue(KinecticEnergyDict);

            //Set to true
            setValueToBool(falseIndexOfKinectic, KinecticEnergyDict, true);
            // transferAnimator.ResetTrigger("energyAnimation");
        }




    }



    private string findFirstFalseValue(Dictionary<GameObject, bool> dict)
    {
        var firstFalseIndex = "0";

        foreach (KeyValuePair<GameObject, bool> entry in dict)
        {
            if (entry.Value == false)
            {
                firstFalseIndex = entry.Key.name;
                break;
            }
        }
        return firstFalseIndex;
    }

    private void setValueToBool(string index, Dictionary<GameObject, bool> dict, bool b)
    {

        foreach (KeyValuePair<GameObject, bool> entry in dict)
        {
            if (entry.Key.name == index)
            {
               dict[entry.Key] = b;
                break;
            }
        }
    }
    public string FindLastTrueValue(Dictionary<GameObject, bool> dict)
    {
        var LastTrueIndex = "-1";
       

        foreach (KeyValuePair<GameObject, bool> entry in dict)
        {
            if (entry.Value == true)
            {
                LastTrueIndex = entry.Key.name;
               
            }
            if (entry.Value == false)
            {
                break;

            }
        }

        if(LastTrueIndex == "-1")
        {
            return null;
        }
        return LastTrueIndex;
    }


}
