﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    void Start()
    {
        GenerateEnemies.Generate(new Vector2Int(4,4),3);
        var ai = new AiPath(new Vector2(0f,0f), "patrol", new Vector2[] { new Vector2(5f,5f)});
        ai.Move(1f);
        Debug.Log($"pos: {ai.Position} x: {ai.Position.x} y: {ai.Position.y}");
        ai.Move(1f);
        Debug.Log($"pos: {ai.Position} x: {ai.Position.x} y: {ai.Position.y}");
        ai.Move(1f);
        Debug.Log($"pos: {ai.Position} x: {ai.Position.x} y: {ai.Position.y}");
        ai.Move(1f);
    }
    void Update()
    {
    }
}
