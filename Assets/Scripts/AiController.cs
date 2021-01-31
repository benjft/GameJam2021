using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    public int id = -1;
    public float speed = 0.03f;
    
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

    //void OnCollisionEnter2D(Collision2D other)
    //{
    //    PlayerController player = other.gameObject.GetComponent<PlayerController>();
    //    //////////////////////////////use this of detection ^^^^^^^^^^^^^
    //    if (player != null)
    //    {
    //        player.ChangeWill(-3);
    //    }
    //}
    float damagePlayer = 0;
    float time = 0;
    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (PlayerSet)
        {
            damagePlayer += aiPath.Move(speed * Time.deltaTime, PlayerLocation) * Time.deltaTime;
        }
        else
            aiPath.Move(speed * Time.deltaTime);
        if(time > 2)
        {
            Debug.Log("HURT ME!");
            time = 0;
            if (damagePlayer != 0)
            {
                Debug.Log("DAMAGE ME!");
                var player = GameObject.FindGameObjectWithTag("Player");
                var playerCon = player.GetComponent<PlayerController>();
                playerCon.TakeDamage(damagePlayer);
            }
            damagePlayer = 0;
        }
        transform.transform.position = aiPath.Position;
    }
}
