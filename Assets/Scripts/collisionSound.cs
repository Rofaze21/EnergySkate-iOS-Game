using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class collisionSound : MonoBehaviour
{
    // Play sound depending upon how big
    // the impact of lofi girl was with chimney

    public Collider2D chimneyCollider;
    public AudioSource audiosource;
    public Slider kinectic;
    bool ChimneyHit = false;
    private int min = 0;
    private int max = 150;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If chimney hit with force betweeen .15 and .70, adjust volume and play it
        // Limit the most loud sound to be .70
        // If lower than .15, play at .05 speed

        if (ChimneyHit == false)
        {
            var normalizedFloat = (kinectic.value - min) / (max - min);
            if (normalizedFloat > .14)
            {
                if (normalizedFloat > .70)
                {
                    audiosource.volume = .70f;

                }
                else
                {
                    audiosource.volume = normalizedFloat;
                }


            }
            else
            {
                audiosource.volume = .05f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      // Play sound upon collision of skateboard and play it only once
        if (ChimneyHit == false)
        {
            if (chimneyCollider.IsTouching(collision))
            {

            if (collision.gameObject.tag == "Skateboard")
            {
                audiosource.Play();
                ChimneyHit = true;

                }
            }
        }
    }
}
