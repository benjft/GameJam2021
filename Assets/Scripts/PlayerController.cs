﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 3.0f;
    public int maxWill = 5;
    public int Will { get { return currentWill; } }

    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    public Sprite oldSprite;

    int currentWill;

    public float timeCamo = 0.0f;
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    //Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    List<AiController> ais;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        oldSprite = spriteRenderer.sprite;
        currentWill = maxWill;
    }
    float time = 0;
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time > 1)
        {
            ais = new List<AiController>();
            foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                ais.Add(obj.GetComponent<AiController>());
            }
        }
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal, vertical);
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        if(Input.GetKey(KeyCode.Space))
        {
            Camouflage();
            
        }
        else
        {
            Reveal();
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;
        if(ais != null)
        {
            foreach (var ai in ais)
            {
                ai.playerLocation = new Vector2Int((int)position.x,(int)position.y);
            }
        }
        rigidbody2d.MovePosition(position);
    }

    public void ChangeWill(int amount)
    {
        if (amount < 0)
        {
           // animator.SetTrigger("Hit");
            
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        //currentWill = Mathf.Clamp(currentWill + amount, 0, maxWill);
        Debug.Log(currentWill + "/" + maxWill);
        currentWill += amount;
        Debug.Log(currentWill + "/" + maxWill);
    }

    void Camouflage()
    {
        if (Will > 1)
        {
            //Changing sprite
            spriteRenderer.sprite = newSprite;
            isInvincible = true;
            
            timeCamo =- Time.deltaTime;
            if (timeCamo <= 0)
            {
                Debug.Log(currentWill + "/" + maxWill);
                ChangeWill(-1);
                Debug.Log(currentWill + "/" + maxWill);
                //timeCamo = 1;
            }
        }
        else
            spriteRenderer.sprite = oldSprite;
    }

    void Reveal()
    {
        spriteRenderer.sprite = oldSprite;
    }



}