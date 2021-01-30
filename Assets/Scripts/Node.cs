using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Node Up { get; set; } = null;
    public Node UpRight { get; set; } = null;
    public Node Right { get; set; } = null;
    public Node DownRight { get; set; } = null;
    public Node Down { get; set; } = null;
    public Node DownLeft { get; set; } = null;
    public Node Left { get; set; } = null;
    public Node UpLeft { get; set; } = null;
    public float Horistic { get; set; }
    public float Cost { get; set; }
    public Vector2 GameObject { get; set; }

    //The distance is essentially the estimated distance, ignoring walls to our target. 
    //So how many tiles left and right, up and down, ignoring walls, to get there. 
    public void SetDistance(Node target)
    {
        Horistic = (float)Math.Sqrt(Math.Pow(target.GameObject.x - GameObject.x, 2) + Math.Pow(target.GameObject.y - GameObject.y, 2));
    }
    public Node GetBestRelatedNode(Node target)
    {
        Node best = null;
        if (Up != null)
        {
            Up.SetDistance(target);
            best = best == null ? Up : best.Horistic > Up.Horistic ? Up : best;
        }
        if (Right != null)
        {
            Right.SetDistance(target);
            best = best == null ? Right : best.Horistic > Right.Horistic ? Right : best;
        }
        if (Down != null)
        {
            Down.SetDistance(target);
            best = best == null ? Down : best.Horistic > Down.Horistic ? Down : best;
        }
        if (Left != null)
        {
            Left.SetDistance(target);
            best = best == null ? Left : best.Horistic > Left.Horistic ? Left : best;
        }
        if (UpRight != null)
        {
            UpRight.SetDistance(target);
            best = best == null ? UpRight : best.Horistic > UpRight.Horistic ? UpRight : best;
        }
        if (DownRight != null)
        {
            DownRight.SetDistance(target);
            best = best == null ? DownRight : best.Horistic > DownRight.Horistic ? DownRight : best;
        }
        if (DownLeft != null)
        {
            DownLeft.SetDistance(target);
            best = best == null ? DownLeft : best.Horistic > DownLeft.Horistic ? DownLeft : best;
        }
        if (UpLeft != null)
        {
            UpLeft.SetDistance(target);
            best = best == null ? UpLeft : best.Horistic > UpLeft.Horistic ? UpLeft : best;
        }
        return best;
    }
}
