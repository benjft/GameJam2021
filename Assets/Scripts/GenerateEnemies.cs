using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    public static void Generate(Vector2Int Space, int count)
    {
        var enemy = new GameObject("Enemy", typeof(SpriteRenderer));
        enemy = GameAssets.instance.Enemy;
        Instantiate (Resources.Load("Enemy") as GameObject);
        Debug.Log("hello");
        //second.name = "real";
        Debug.Log("hello2");
    }
}
