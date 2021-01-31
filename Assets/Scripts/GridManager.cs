using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;


public class GridManager : MonoBehaviour {
    [Header("Dimensions")]
    public int rows = 50;
    public int cols = 50;
    public float scale = 1f;
    [Header("Node Data")]
    public NodeMap NodeMap;
    bool[,] getMaze() {
        var tiles = new bool[cols, rows];

        var startX = (int) (Random.value * cols);
        var startY = (int) (Random.value * rows);
        NodeMap = new NodeMap();

        var path = new Stack<Vector2Int>();
        var current = new Vector2Int(startX, startY);
        NodeMap.Nodes.Add($"{current}", new Node() { Position = current });
        while (current != null) {
            tiles[current.x, current.y] = true;
            var valid = new List<Vector2Int>();
            if (current.x > 0 && !tiles[current.x - 1, current.y]) {
                if ((current.x == 1 || !tiles[current.x - 2, current.y]) &&
                    (current.y == 0 || !tiles[current.x - 1, current.y - 1]) &&
                    (current.y == rows - 1 || !tiles[current.x - 1, current.y + 1])) {
                    valid.Add(new Vector2Int(current.x - 1, current.y));
                }
            }

            if (current.y > 0 && !tiles[current.x, current.y - 1]) {
                if ((current.y == 1 || !tiles[current.x, current.y - 2]) &&
                    (current.x == 0 || !tiles[current.x - 1, current.y - 1]) &&
                    (current.x == cols - 1 || !tiles[current.x + 1, current.y - 1])) {
                    valid.Add(new Vector2Int(current.x, current.y - 1));
                }
            }

            if (current.x < cols - 1 && !tiles[current.x + 1, current.y]) {
                if ((current.x == cols - 2 || !tiles[current.x + 2, current.y]) &&
                    (current.y == 0 || !tiles[current.x + 1, current.y - 1]) &&
                    (current.y == rows - 1 || !tiles[current.x + 1, current.y + 1])) {
                    valid.Add(new Vector2Int(current.x + 1, current.y));
                }
            }

            if (current.y < rows - 1 && !tiles[current.x, current.y + 1]) {
                if ((current.y == rows - 2 || !tiles[current.x, current.y + 2]) &&
                    (current.x == 0 || !tiles[current.x - 1, current.y + 1]) &&
                    (current.x == cols - 1 || !tiles[current.x + 1, current.y + 1])) {
                    valid.Add(new Vector2Int(current.x, current.y + 1));
                }
            }

            if (valid.Count == 0) {
                if (path.Count > 0) {
                    current = path.Pop();
                } else {
                    break;
                }
                continue;
            }
            
            path.Push(current);
            var temp = valid[Random.Range(0, valid.Count)];
            NodeMap.Nodes.Add($"{temp}", new Node() { Position = temp });
            setRelatedNodes($"{current}", $"{temp}");
            current = temp;
        }

        return tiles;

        void setRelatedNodes(string fromId, string toId)
        {
            //Debug.Log($"hereweare {NodeMap.Nodes.ContainsKey($"{toId}")}");
            if (NodeMap.Nodes.ContainsKey($"{toId}"))
            {
                NodeMap.Nodes[toId].RelatedNodes.Add(NodeMap.Nodes[fromId]);
                NodeMap.Nodes[fromId].RelatedNodes.Add(NodeMap.Nodes[toId]);
            }

        }
    }
    void GenerateLevel() {
        var tileWall = Resources.Load<GameObject>("Tiles/TileWall");
        var tileFloor = Resources.Load<GameObject>("Tiles/TileFloor");
        tileFloor.tag = "Floor";
        var tileStart = Resources.Load<GameObject>("Tiles/TileStart");
        var tileEnd = Resources.Load<GameObject>("Tiles/TileEnd");
        var maze = getMaze();
        
        for (var row = 0; row < rows; row++) {
            for (var col = 0; col < cols; col++) {
                var posX = col * scale;
                var posY = -row * scale;
                GameObject tile;
                if (row == 0 && col == 0) {
                    tile = Instantiate(tileStart, transform);
                } else if (row == rows-1 && col == cols-1) {
                    tile = Instantiate(tileEnd, transform);
                } else {
                    tile = Instantiate(maze[col, row] ? tileFloor : tileWall, transform);
                }
                tile.transform.position = new Vector3(posX, posY);
            }
        }

        var gridW = cols * scale;
        var gridH = rows * scale;
        transform.position = new Vector3(-gridW / 2, gridH / 2);
    }
    
    void Start() {
        NodeMap = new NodeMap();
        GenerateLevel();
    }
}
