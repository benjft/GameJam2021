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
    private bool vertical; // vertical has been irrelevent
    private AiPath aiPath;
    
    void Start()
    {
        aiPath = new AiPath(Position, "Patrol", new Vector2[] { new Vector2(5, 5), new Vector2(2, 2) });
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
            //CheckDetection()
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        //////////////////////////////use this of detection ^^^^^^^^^^^^^
        if (player != null)
        {
            player.ChangeWill(-1);
            
        }
    }

    void FixedUpdate()
    {
        aiPath.Move(speed);
        
        rigidbody2d.MovePosition(aiPath.Position);
    }


}
