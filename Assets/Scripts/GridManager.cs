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
    
    bool[,] getMaze() {
        var tiles = new bool[cols, rows];

        var startX = (int) (Random.value * cols);
        var startY = (int) (Random.value * rows);


        var path = new Stack<IntPair>();
        var current = new IntPair(startX, startY);
        while (current != null) {
            tiles[current.x, current.y] = true;
            var valid = new List<IntPair>();
            if (current.x > 0 && !tiles[current.x - 1, current.y]) {
                if ((current.x == 1 || !tiles[current.x - 2, current.y]) &&
                    (current.y == 0 || !tiles[current.x - 1, current.y - 1]) &&
                    (current.y == rows - 1 || !tiles[current.x - 1, current.y + 1])) {
                    valid.Add(new IntPair(current.x - 1, current.y));
                }
            }

            if (current.y > 0 && !tiles[current.x, current.y - 1]) {
                if ((current.y == 1 || !tiles[current.x, current.y - 2]) &&
                    (current.x == 0 || !tiles[current.x - 1, current.y - 1]) &&
                    (current.x == cols - 1 || !tiles[current.x + 1, current.y - 1])) {
                    valid.Add(new IntPair(current.x, current.y - 1));
                }
            }

            if (current.x < cols - 1 && !tiles[current.x + 1, current.y]) {
                if ((current.x == cols - 2 || !tiles[current.x + 2, current.y]) &&
                    (current.y == 0 || !tiles[current.x + 1, current.y - 1]) &&
                    (current.y == rows - 1 || !tiles[current.x + 1, current.y + 1])) {
                    valid.Add(new IntPair(current.x + 1, current.y));
                }
            }

            if (current.y < rows - 1 && !tiles[current.x, current.y + 1]) {
                if ((current.y == rows - 2 || !tiles[current.x, current.y + 2]) &&
                    (current.x == 0 || !tiles[current.x - 1, current.y + 1]) &&
                    (current.x == cols - 1 || !tiles[current.x + 1, current.y + 1])) {
                    valid.Add(new IntPair(current.x, current.y + 1));
                }
            }

            if (valid.Count == 0) {
                if (path.Count > 0) {
                    current = path.Pop();
                } else {
                    current = null;
                }
                continue;
            }
            
            path.Push(current);

            current = valid[Random.Range(0, valid.Count)];
        }

        return tiles;
    }

    void GenerateLevel() {
        var tileWall = Resources.Load<GameObject>("Tiles/TileWall");
        var tileFloor = Resources.Load<GameObject>("Tiles/TileFloor");
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
        GenerateLevel();
    }
}

internal class IntPair {
    internal readonly int x, y;
    internal IntPair(int x, int y) {
        this.x = x;
        this.y = y;
    }
}
