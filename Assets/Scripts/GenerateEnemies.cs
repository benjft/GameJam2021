using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    // patrol
    // pursuit
    // search
    public static void Generate(Vector2Int StartPosition, int id)
    {
        var enemy = Resources.Load("Prefabs/Enemy") as GameObject;
        enemy.name = $"enemy {id}";
        AiController obj = enemy.GetComponent<AiController>();
        Debug.Log("here1");
        //obj.Position = new Vector2(1, 3);
        Debug.Log("here2");
        Instantiate(obj);
    }
}
