using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    public int id = -1;
    public float speed = 3f;
    
    public List<Vector2> Patrol = new List<Vector2>();
    public float changeTime = 3.0f;
    
    Rigidbody2D rigidbody2d; // new added to remove warning

    float timer;
    public bool PlayerSet = false;
    public Vector2Int PlayerLocation;
    public AiPath aiPath { get; set; }
    void Start()
    {
        var points = new List<Vector2Int>();
        var route = new List<Vector2Int>();
        GenerateEnemies.CreateAndObtainPath(out points, out route);
        aiPath = new AiPath(points, route);
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
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
            player.ChangeWill(-3);
        }
    }
    void FixedUpdate()
    {
        if (PlayerSet)
        {
            aiPath.Move(speed * Time.deltaTime, PlayerLocation);
        }
        else
            aiPath.Move(speed * Time.deltaTime);
        transform.transform.position = aiPath.Position;
    }
}
