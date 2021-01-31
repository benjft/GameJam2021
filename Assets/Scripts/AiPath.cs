using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPath
{

    public Vector2 Position { get; private set; }
    private Vector2Int playerLastSeen;
    private bool playerSeenSet = false;
    public string PathState { get; set; }
    public List<Vector2Int> PatrolPoints { get; private set; }
    private Vector2Int destination;
    private int patrolPointsi = 1;
    private bool patrolState = true;
    public List<Vector2Int> Path { get; private set; }
    private int pathPointsi = 1;
    public AiPath(List<Vector2Int> patrolPoints, List<Vector2Int> Path)
    {
        Position = patrolPoints[0];
        PatrolPoints = patrolPoints;
    }
    public void Move(float speed, Vector2Int playerLastSeen)
    {
        if (stop) return;
        this.playerLastSeen = playerLastSeen;
        playerSeenSet = true;
        if (checkPlayer()) 
            pursuit(speed);
        else
            patrol(speed);
    }
    public void Move(float speed)
    {
        patrol(speed);
    }
    private bool checkPlayer()
    {

        if(Math.Abs(playerLastSeen.x - (int)Position.x + playerLastSeen.y - (int)Position.y) < 3 && playerSeenSet)
        {
            patrolState = false;
            return true;
        }
        return false;
    }
    private void patrol(float speed)
    {
        if (!patrolState)
        {
            var posAsInt = new Vector2Int((int)Position.x, (int)Position.y);
            destination = PatrolPoints[patrolPointsi];
            setPathPoints(posAsInt, destination);
            patrolState = true;
        }
        if(Path == null)
        {
            var posAsInt = new Vector2Int((int)Position.x, (int)Position.y);
            destination = PatrolPoints[patrolPointsi];
            setPathPoints(posAsInt, destination);
            patrolPointsi++;
            if (patrolPointsi == PatrolPoints.Count) patrolPointsi = 0;
        }
        if (Path == null) { Debug.Log("null path patrol"); patrolPointsi++; return; }
        // increase position
        if (Path.Count <= 1)
        {
            Path = null;
            return;
        }
        Vector2 newPos = new Vector2(setVal(speed, Position.x, Path[pathPointsi].x), setVal(speed, Position.y, Path[pathPointsi].y));
        // if it has made it to the path
        if (newPos == Path[pathPointsi])
        {
            pathPointsi++;
            // reset to starting path position
            if (Path.Count <= pathPointsi) { Path = null; pathPointsi = 1; }
        }
        Position = newPos;
    }
    bool stop = false;
    private void pursuit(float speed)
    {
        setPathPoints(new Vector2Int((int)Position.x, (int)Position.y), playerLastSeen);
        if (Path == null) { Debug.Log("hereio"); return; }
        pathPointsi = 0;
        if (Path.Count == 0) return;
        var newPos = new Vector2(setVal(speed, Position.x, Path[pathPointsi].x), setVal(speed, Position.y, Path[pathPointsi].y));
        if (newPos == Path[pathPointsi])
        {
            pathPointsi++;
            // reset to starting path position
            if (Path.Count <= pathPointsi) { stop = true; Debug.Log("fini"); }
        }
        Position = newPos;
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
    private void setPathPoints(Vector2Int from, Vector2Int to)
    {
        Path = GenerateEnemies.ObtainPath(from, to);
        
    }
}
