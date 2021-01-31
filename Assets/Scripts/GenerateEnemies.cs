using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    public static void Generate()
    {
        var enemy = Resources.Load("Prefabs/darknessF") as GameObject;
        AiController obj = enemy.GetComponent<AiController>();
        Instantiate(obj);
        Debug.Log("enemy created");
    }
    private void setPathPoints(Vector2Int from, Vector2Int to)
    {
    }
}
