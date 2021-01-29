using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPath
{
    public Vector2 Position { get; private set; }
    public string PathState { get; set; }
    public Vector2[] Pathing { get; private set; }
    private int pathi = 0;
    public AiPath(Vector2 position, string pathState, Vector2[] pathing)
    {
        Position = position;
        PathState = pathState;
        Pathing = pathing;
    }
    public void Move(float speed)
    {
        if (PathState == "patrol") Patrol(speed);
    }
    private void Patrol(float speed)
    {
        if (Position.x < Pathing[pathi].x) Position = new Vector2(Position.x + speed, Position.y);
        else if(Position.x > Pathing[pathi].x) Position = new Vector2(Position.x - speed, Position.y);
        if (Position.y < Pathing[pathi].y) Position = new Vector2(Position.x, Position.y + speed);
        else if(Position.y > Pathing[pathi].y) Position = new Vector2(Position.x, Position.y - speed);
        if (Position.x == Pathing[pathi].x && Position.y == Pathing[pathi].y)
        {
            pathi++;
            if (pathi == Pathing.Length) pathi = 0;
        }
    }
}
