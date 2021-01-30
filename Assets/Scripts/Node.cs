using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node Parent { get; set; }
    public GameObject Object { get; set; }

    //The distance is essentially the estimated distance, ignoring walls to our target. 
    //So how many tiles left and right, up and down, ignoring walls, to get there. 
    //public void SetDistance(int targetX, int targetY)
    //{
    //    this.Distance = Math.Abs(targetX - Object.transform.position.x) + Math.Abs(targetY - Object.transform.position.y);
    //}
}
