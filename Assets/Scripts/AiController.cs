using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    public float speed = 3.0f;
    Rigidbody2D rigidbody2d;
    private float xDirection;
    private float yDirection;

    void Start()
    {
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
