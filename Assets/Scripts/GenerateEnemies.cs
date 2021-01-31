using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GenerateEnemies : MonoBehaviour
{
    public static void Generate()
    {
        var enemy = Resources.Load("Prefabs/Beeg") as GameObject;
        AiController obj = enemy.GetComponent<AiController>();
        Instantiate(obj);
        Debug.Log("enemy created");
    }
    public static void Generate2(int enemyId)
    {
        var enemy = Resources.Load("Prefabs/grim") as GameObject;
        AiController obj = enemy.GetComponent<AiController>();
        obj.id = enemyId;
        Instantiate(obj);
        Debug.Log("enemy created");
    }
    public static List<Vector2Int> ObtainPath(Vector2Int from, Vector2Int to)
    {
        var map = new NodeMap();
        foreach (var tile in GameObject.FindGameObjectsWithTag("Floor"))
        {
            map.Nodes.Add($"{new Vector2Int((int)tile.transform.position.x, (int)tile.transform.position.y)}",
                new Node()
                {
                    Position = new Vector2Int((int)tile.transform.position.x,
                    (int)tile.transform.position.y)
                });
        }
        return map.FastestRoute($"{from}", $"{to}");
    }
    public static void CreateAndObtainPath(out List<Vector2Int> points, out List<Vector2Int> route)
    {
        var map = new NodeMap();
        points = new List<Vector2Int>();

        foreach (var tile in GameObject.FindGameObjectsWithTag("Floor"))
        {
            map.Nodes.Add($"{new Vector2Int((int)tile.transform.position.x, (int)tile.transform.position.y)}",
                new Node()
                {
                    Position = new Vector2Int((int)tile.transform.position.x,
                    (int)tile.transform.position.y)
                });
        }
        points.Add(map.Nodes.ElementAt(Random.Range(0, map.Nodes.Count - 1)).Value.Position);
        points.Add(map.Nodes.ElementAt(Random.Range(0, map.Nodes.Count - 1)).Value.Position);
        points.Add(map.Nodes.ElementAt(Random.Range(0, map.Nodes.Count - 1)).Value.Position);
        points.Add(map.Nodes.ElementAt(Random.Range(0, map.Nodes.Count - 1)).Value.Position);
        route = map.FastestRoute($"{points[0]}", $"{points[1]}");
    }
}