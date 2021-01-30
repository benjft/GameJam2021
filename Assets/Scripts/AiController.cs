using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    public float speed = 3.0f;
    public Vector2 Position = new Vector2(0,0);
    
    public List<Vector2> Patrol = new List<Vector2>();
    public float changeTime = 3.0f;
    
    Rigidbody2D rigidbody2d; // new added to remove warning

    float timer;
    int direction = 1;
    

    private float Direction;
    private bool vertical;

    void Start()
    {
        timer = changeTime;
        rigidbody2d = GetComponent<Rigidbody2D>();
        Direction = (float)Random.Range(0f, 1f); // random direction up or down random range is range
        if (Direction >= 0.5f)
        {
            vertical = true;
            Debug.Log("Vertical");
        }

        else
        {
            vertical = false;
            Debug.Log("Horizontal");
        }

    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.ChangeWill(-1);
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
            position.y += Time.deltaTime * speed * direction;
            //animator.SetFloat("Move X", 0);
            //animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x += Time.deltaTime * speed * direction;
            //animator.SetFloat("Move X", direction);
            //animator.SetFloat("Move Y", 0);
        }

        rigidbody2d.MovePosition(position);
    }
}
