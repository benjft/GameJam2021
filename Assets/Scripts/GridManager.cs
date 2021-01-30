using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

internal enum EnumTiles {
    
}

public class GridManager : MonoBehaviour {
    [Header("Dimensions")]
    public int rows = 11;
    public int cols = 11;
    public float scale = 1f;



    void GenerateLevel() {
        var tileWall = Resources.Load<GameObject>("Tiles/TileWall");
        var tileFloor = Resources.Load<GameObject>("Tiles/TileFloor");
        
        for (var row = 0; row < rows; row++) {
            for (var col = 0; col < cols; col++) {
                var posX = col * scale;
                var posY = -row * scale;

                var isFloor = Random.value < 0.5;
                var tile = Instantiate(isFloor ? tileFloor : tileWall, transform);
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
