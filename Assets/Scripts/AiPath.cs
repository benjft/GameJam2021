using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPath
{
    public Vector2 Position { get; private set; }
    public Vector2Int PlayerLastSeen { get; private set; }
    public string PathState { get; set; }
    public List<Vector2Int> PatrolPoints { get; private set; }
    private Vector2Int destination;
    private int patrolPointsi = 1;
    public List<Vector2Int> Path { get; private set; }
    private int pathPointsi = 1;
    //public AiPath(List<Node> patrolPoints, string pathState = "patrol")
    //{
    //    Position = patrolPoints[0].Position;
    //    PathState = pathState;
    //    PatrolPoints = patrolPoints;
    //}
    public AiPath(List<Vector2Int> patrolPoints, List<Vector2Int> Path, string pathState = "patrol")
    {
        Position = patrolPoints[0];
        PathState = pathState;
        PatrolPoints = patrolPoints;
    }
    public void Move(float speed)
    {
        if (PathState == "patrol") patrol(speed);
        //else if (PathState == "pursuit") pursuit(speed);
    }
    private void patrol(float speed)
    {
        if(Path == null)
        {
            var posAsInt = new Vector2Int((int)Position.x, (int)Position.y);
            destination = PatrolPoints[patrolPointsi];
            setPathPoints(posAsInt, destination);
            patrolPointsi++;
            if (patrolPointsi == PatrolPoints.Count) patrolPointsi = 0;
        }
        // increase position
        Vector2 newPos = new Vector2(-1000,-1000);
        // handle no movement situations
        try
        {
            newPos = new Vector2(setVal(speed, Position.x, Path[pathPointsi].x),
                setVal(speed, Position.y, Path[pathPointsi].y));
        }
        catch (System.Exception)
        {
            pathPointsi = 0;
        }
        // if it has made it to the path
        if (newPos == Path[pathPointsi])
        {
            pathPointsi++;
            // reset to starting path position
            if (Path.Count <= pathPointsi) { Path = null; pathPointsi = 1; }
        }
        Position = newPos;
    }
    //private void pursuit(float speed)
    //{
    //    Position = new Vector2(setVal(speed, Position.x, PlayerLastSeen.x), setVal(speed, Position.y, PlayerLastSeen.y));
    //}
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
    private void setPathPoints(Vector2Int from, Vector2Int to)
    {
        Path = GenerateEnemies.ObtainPath(from, to);
    }
}
