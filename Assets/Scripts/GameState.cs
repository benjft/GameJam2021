using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    void Start()
    {
        GenerateEnemies.Generate(new Vector2Int(4,4),3);
    }
    void Update()
    {
    }
    private void createGround()
    {

    }
}
