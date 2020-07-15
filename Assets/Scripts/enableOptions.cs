using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableOptions : MonoBehaviour
{
    //Eable options when lose conditions come up (Try Again, Quit)

	public GameObject options;
	public GameObject selfObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(selfObject.activeSelf)
		{
			options.SetActive(true);
		}


        if (selfObject.activeSelf == false)
        {
            options.SetActive(false);
        }
    }
}
