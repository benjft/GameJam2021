using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPath
{
    public Vector2 Position { get; private set; }
    public Vector2 PlayerLastSeen { get; private set; }
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
        if (PathState == "patrol") patrol(speed);
        else if (PathState == "pursuit") pursuit(speed);
    }
    private void patrol(float speed)
    {
        // increase position
        var newPos = new Vector2(setVal(speed, Position.x, Pathing[pathi].x), setVal(speed, Position.y, Pathing[pathi].y));
        // if it has made it to the path
        if (newPos == Pathing[pathi])
        {
            // increase the path
            pathi++;
            // reset to starting path position
            if (pathi == Pathing.Length) pathi = 0;
        }
        Position = newPos;
    }
    private void pursuit(float speed)
    {
        Position = new Vector2(setVal(speed, Position.x, PlayerLastSeen.x), setVal(speed, Position.y, PlayerLastSeen.y));
    }
    private float setVal(float speed, float xy, float comp)
    {
        // increase/decrease the x/y by the speed unless it goes beyond the bounds of the path position
        if (xy < comp)
        {
            if (xy + speed > comp)
            {
                return comp;
            }
            else
            {
                return xy + speed;
            }
        }
        if (xy > comp)
        {
            if (xy - speed < comp)
            {
                return comp;
            }
            else
            {
                return xy - speed;
            }
        }
        return xy;
    }
}
