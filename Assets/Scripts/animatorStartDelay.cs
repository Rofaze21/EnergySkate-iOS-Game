using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class animatorStartDelay : MonoBehaviour
{
    public Animator animator;

    public float delayInSeconds = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("playAnimation");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator playAnimation()
    {

        animator.Play("flicker");

        yield return new WaitForSeconds(delayInSeconds);

    }

}
