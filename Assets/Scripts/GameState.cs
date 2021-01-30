using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    List<Node> Map;
    void Start()
    {
        //GenerateEnemies.Generate(new Vector2Int(4,4),3);
        //var ai = new AiPath(new Vector2(0f,0f), "patrol", new Vector2[] { new Vector2(5f,5f), new Vector2(0f, 0f) });
        var lst = new List<Vector2>() {
            new Vector2(),
            new Vector2(),
            new Vector2(),
            new Vector2(),
            new Vector2(),
            new Vector2(),
        };
        //var lst = new List<GameObject>() {
        //    Resources.Load("Prefabs/HollowTile") as GameObject,
        //    Resources.Load("Prefabs/HollowTile") as GameObject,
        //    Resources.Load("Prefabs/HollowTile") as GameObject,
        //    Resources.Load("Prefabs/HollowTile") as GameObject,
        //    Resources.Load("Prefabs/HollowTile") as GameObject,
        //    Resources.Load("Prefabs/HollowTile") as GameObject
        //};
        //for (int i = 0; i < 3; i++)
        //{
        //    for (int j = 0; j < 3; j++)
        //    {
        //        lst[i].transform.position = new Vector3(i, j, 0);
        //    }
        //}
        var nodes = new List<Node>();
        for (int i = 0; i < 6; i++)
        {
            nodes.Add(new Node() {  });
        }

        nodes[0].GameObject = new Vector2(0f, 0f);
        nodes[1].GameObject = new Vector2(0f, 1f);
        nodes[2].GameObject = new Vector2(0f, 2f);
        nodes[3].GameObject = new Vector2(1f, 0f);
        nodes[4].GameObject = new Vector2(1f, 1f);
        nodes[5].GameObject = new Vector2(1f, 2f);
        for (int i = 0; i < lst.Count; i++)
        {
            nodes.Add(new Node() { GameObject = lst[i] });
        }
        nodes[0].Down = nodes[3];
        nodes[0].Right = nodes[1];
        nodes[1].Left = nodes[0];
        nodes[1].Right = nodes[2];
        nodes[1].Down = nodes[4];
        nodes[1].DownLeft = nodes[3];
        nodes[1].DownRight = nodes[5];
        nodes[2].Left = nodes[1];
        nodes[2].Down = nodes[5];
        nodes[2].DownLeft = nodes[4];
        nodes[3].Up = nodes[0];
        nodes[3].Right = nodes[4];
        nodes[3].UpRight = nodes[1];
        nodes[4].Up = nodes[1];
        nodes[4].Right = nodes[5];
        nodes[4].Left = nodes[4];
        nodes[4].UpRight = nodes[2];
        nodes[4].UpLeft = nodes[0];
        nodes[5].Up = nodes[2];
        nodes[5].Left = nodes[4];
        nodes[5].UpLeft = nodes[1];
        var nodeMap = new NodeMap(nodes);
        var ret = nodeMap.FastestRoute(nodes[0], nodes[5]);
        Debug.Log("nodes:");
        foreach (var item in ret)
        {
            Debug.Log($"pos: {item.GameObject.x}/{item.GameObject.y}");
        }
        Debug.Log($"pos: {nodes[5].GameObject.x}/{nodes[5].GameObject.y}");
        //ai.Move(2f);
        //Debug.Log($"pos: {ai.Position} x: {ai.Position.x} y: {ai.Position.y}");
    }
    void Update()
    {
    }
}
