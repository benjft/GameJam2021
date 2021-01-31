using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPath
{
    public Vector2 Position { get; private set; }
    public Vector2Int PlayerLastSeen { get; private set; }
    public string PathState { get; set; }
    public List<Node> PatrolPoints { get; private set; }
    private int patrolPointsi = 1;
    public List<Vector2Int> Path { get; private set; }
    private int pathPointsi = 1;
    public AiPath(List<Node> patrolPoints, string pathState = "patrol")
    {
        Position = patrolPoints[0].Position;
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
        Debug.Log("enemy patrol");
        if(Path == null)
        {
            pathPointsi = patrolPointsi + 1 == PatrolPoints.Count ? 0 : patrolPointsi + 1;
            setPathPoints(PatrolPoints[patrolPointsi].Position,
                patrolPointsi == PatrolPoints.Count ? PatrolPoints[patrolPointsi + 1].Position : 
                PatrolPoints[0].Position);
        }
        // increase position
        var newPos = new Vector2(setVal(speed, Position.x, Path[pathPointsi].x), 
            setVal(speed, Position.y, Path[pathPointsi].y));
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
        var GridManagerObject = GameObject.FindWithTag("Map");
        if (GridManagerObject != null)
        {
            var GridManager = GridManagerObject.GetComponent<GridManager>();
            Debug.Log("getting enemy path");
            Path = GridManager.NodeMap.FastestRoute($"{from}", $"{to}");
            Debug.Log("enemy path obtained");
            if (Path == null) throw new System.Exception($"Error with Node id {from} and {to}");
        }
    }
}
