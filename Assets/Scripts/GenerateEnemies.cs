using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    // patrol
    // pursuit
    // search
    public static void Generate(Vector2 StartPosition, int id)
    {
        var enemy = Resources.Load("Prefabs/darknessF") as GameObject;
        enemy.name = $"enemy {id}";
        AiController obj = enemy.GetComponent<AiController>();
        //obj.Position = new Vector2(1, 3);
        Instantiate(obj);
    }
}