using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int max_willpower = 100;
    public int current_willpower;

    public Willpower willbar;
    public Willpower willbar2;
    // Start is called before the first frame update
    void Start()
    {
        current_willpower = max_willpower;
        willbar.SetMaxWillpower(max_willpower);
        willbar2.SetMaxWillpower(max_willpower);
        willbar2.SetWillpower(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }


    }
    void TakeDamage(int damage)
    {
        current_willpower -= damage;

        willbar.SetWillpower(current_willpower);
        willbar2.SetWillpower(max_willpower - current_willpower);
    }
}
