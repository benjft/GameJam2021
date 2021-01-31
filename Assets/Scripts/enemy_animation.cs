using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_animation : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.magnitude == 0)
        {
            anim.SetFloat("movement", 0f);
        }
        else if(rb.velocity.x > 0)
        {
            anim.SetFloat("movement", 1f);
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
            }
        }
        else if (rb.velocity.x < 0)
        {
            anim.SetFloat("movement", 1f);
            if(transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
            }
        }
        else if(rb.velocity.y > 0)
        {
            anim.SetFloat("movement", 0.7f);
        }
        else if(rb.velocity.y < 0)
        {
            anim.SetFloat("movement", 0.3f);
        }


    }
}
