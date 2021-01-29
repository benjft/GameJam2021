using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    public static float[,] Generate(int width, int height, float scale, Wave[] waves, Vector2 offset) {
        return null;
    }
}

[System.Serializable]
public class Wave {
    public float seed,
        frequency,
        amplitude;
}