using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public HourGlass TopGlass, BottomGlass;
    public float speed = 3.0f;
    public int maxWill = 5;

    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    public Sprite oldSprite;

    public float timeCamo = 0.0f;
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    public Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    float current_willpower = 100;
    //Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    Dictionary<int, AiController> ais;

    // Start is called before the first frame update
    void Start()
    {
        BottomGlass.flip = true;
        current_willpower = 100;
        TopGlass.SetMaxWillpower(100);
        BottomGlass.SetMaxWillpower(100);
        rigidbody2d = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        oldSprite = spriteRenderer.sprite;
        current_willpower = maxWill;

        ais = new Dictionary<int, AiController>();
        foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Debug.Log($"save");
            var item = obj.GetComponent<AiController>();
            item.PlayerLocation = new Vector2Int((int)rigidbody2d.position.x, (int)rigidbody2d.position.y);
            item.PlayerSet = true;
            if(!ais.ContainsKey(item.id))
                ais.Add(item.id, item);
            else
                ais[item.id].PlayerLocation = new Vector2Int((int)rigidbody2d.position.x, (int)rigidbody2d.position.y);
        }
    }
    float time = 0;
    
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 0.5)
        {
            time = 0;
            foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                var item = obj.GetComponent<AiController>();
                item.PlayerLocation = new Vector2Int((int)rigidbody2d.position.x, (int)rigidbody2d.position.y);
                item.PlayerSet = true;
                if (!ais.ContainsKey(item.id))
                    ais.Add(item.id, item);
                else
                    ais[item.id].PlayerLocation = new Vector2Int((int)rigidbody2d.position.x, (int)rigidbody2d.position.y);
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
            //Camouflage();
        }
        else
        {
            Reveal();
        }
    }
    public void Place(Vector2Int place) => rigidbody2d.MovePosition(place);
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }
    //public void ChangeWill(int amount)
    //{
    //    if (amount < 0)
    //    {
    //       // animator.SetTrigger("Hit");
    //        if (isInvincible)
    //            return;

    //        isInvincible = true;
    //        invincibleTimer = timeInvincible;
    //    }

    //    //currentWill = Mathf.Clamp(currentWill + amount, 0, maxWill);
    //    currentWill += amount;
    //}
    //void Camouflage()
    //{
    //    if (Will > 1)
    //    {
    //        //Changing sprite
    //        spriteRenderer.sprite = newSprite;
    //        isInvincible = true;
            
    //        timeCamo =- Time.deltaTime;
    //        if (timeCamo <= 0)
    //        {
    //            ChangeWill(-1);
    //            //timeCamo = 1;
    //        }
    //    }
    //    else
    //        spriteRenderer.sprite = oldSprite;
    //}
    void Reveal()
    {
        spriteRenderer.sprite = oldSprite;
    }
    public void TakeDamage(float dmg)
    {
        TopGlass.Reduce(dmg);
        BottomGlass.Reduce(dmg);
    }
}