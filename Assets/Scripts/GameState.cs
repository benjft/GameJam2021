using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    GameObject GridManagerObject;
    void Start()
    {
        StartCoroutine(GenerateContent());
    }

    IEnumerator GenerateContent()
    {
        Debug.Log("creating enemy");
        yield return new WaitForSeconds(1);
        GenerateEnemies.Generate2();
        //GenerateEnemies.Generate2();
        //GenerateEnemies.Generate2();
        //GenerateEnemies.Generate2();
    }
}