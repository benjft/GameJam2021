﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GenerateEnemies.Generate(new Vector2Int(4,4),1);
        Debug.Log("here");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
