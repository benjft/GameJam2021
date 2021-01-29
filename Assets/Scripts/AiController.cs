using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    public float speed = 3.0f;
    public Vector2 Position = new Vector2(0,0);
    public List<Vector2> Patrol = new List<Vector2>();
    Rigidbody2D rigidbody2d;
    private float xDirection;
    private float yDirection;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        xDirection = (float)Random.Range(0f, 0.3f);
        yDirection = (float)Random.Range(0f, 0.3f);
    }

    void Update()
    {
        
    }
    void FixedUpdate()
    {
        var position = rigidbody2d.position;
        position.x += speed * xDirection * Time.deltaTime;
        position.y += speed * yDirection * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }
}
