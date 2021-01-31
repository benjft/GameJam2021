using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // related nodes into a lst
    public HashSet<Node> RelatedNodes { get; set; } = new HashSet<Node>();
    public Node Parent { get; set; } = null;
    public float Horistic { get; set; }
    public float MoveCost { get; set; }
    public float HoristicMoveCost { get => Horistic + MoveCost; }
    public Vector2Int Position { get; set; }
    //public GameObject GameObject { get; set; }

    //The distance is essentially the estimated distance, ignoring walls to our target. 
    //So how many tiles left and right, up and down, ignoring walls, to get there. 
    public void SetDistance(Node target)
    {
        Horistic = (float)Math.Sqrt(Math.Pow(target.Position.x - Position.x, 2) + 
            Math.Pow(target.Position.y - Position.y, 2));
    }
    public Node GetBestRelatedNode(Node target)
    {
        Node best = null;
        foreach (var node in RelatedNodes)
        {
            node.SetDistance(target);
            best = best == null ? node : best.Horistic > node.Horistic ? node : best;
        }
        return best;
    }
}
