using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_movement : MonoBehaviour
{
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector2(0f, 1f);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector2(0f, -1f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(1f, 0f);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-1f, 0f);
        }
    }
}
