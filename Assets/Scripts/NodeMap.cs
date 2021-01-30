using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMap
{
    public List<Node> Nodes { get; set; }
    public NodeMap(List<Node> nodes)
    {
        nodes = Nodes;
    }
    public List<Node> FastestRoute(Node start, Node finish)
    {
        start.SetDistance(finish);
        var activeTile = start;
        var visitedTiles = new List<Node>();
        while (activeTile.GameObject.x != finish.GameObject.x ||
            activeTile.GameObject.y != finish.GameObject.y)
        {
            visitedTiles.Add(activeTile);

            activeTile = activeTile.GetBestRelatedNode(finish);

        }
        return visitedTiles;
    }
}
