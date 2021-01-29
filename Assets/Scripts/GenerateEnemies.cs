using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    public static void Generate(Vector2Int Space, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var enemy = Resources.Load("Prefabs/Enemy") as GameObject;
            enemy.transform.position = new Vector3(Random.Range(0, Space.x), Random.Range(0, Space.y), 0);
            enemy.name = $"enemyy {i}";
            Instantiate(enemy);
        }
    }
}
