using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMap
{
    public Dictionary<string, Node> Nodes { get; set; }
    public NodeMap() => Nodes = new Dictionary<string, Node>();
    public NodeMap(Dictionary<string, Node> nodes) => Nodes = nodes;
    public List<Node> FastestRoute(Node start, Node finish)
    {
        start.SetDistance(finish);
        var activeTile = start;
        var visitedTiles = new List<Node>();
        //Debug.Log($"{activeTile.GameObject.transform.position.x} != {finish.GameObject.transform.position.x} + {activeTile.GameObject.transform.position.y} != {finish.GameObject.transform.position.y}");
        while (activeTile.Position.x != finish.Position.x ||
            activeTile.Position.y != finish.Position.y)
        {
            visitedTiles.Add(activeTile);

            activeTile = activeTile.GetBestRelatedNode(finish);

        }
        return visitedTiles;
    }
    public List<Vector2Int> FastestRoute(Vector2Int startId, Vector2Int finishId)
    {
        Node start, finish;
        if (!Nodes.TryGetValue($"{startId}", out start))
            if (!Nodes.TryGetValue($"{new Vector2Int(startId.x + 1, startId.y)}", out start))
                if (!Nodes.TryGetValue($"{new Vector2Int(startId.x, startId.y + 1)}", out start))
                    if (!Nodes.TryGetValue($"{new Vector2Int(startId.x - 1, startId.y)}", out start))
                        if (!Nodes.TryGetValue($"{new Vector2Int(startId.x, startId.y - 1)}", out start))
                            return null;
        if (!Nodes.TryGetValue($"{finishId}", out finish))
            if (!Nodes.TryGetValue($"{new Vector2Int(finishId.x + 1, finishId.y)}", out finish))
                if (!Nodes.TryGetValue($"{new Vector2Int(finishId.x, finishId.y + 1)}", out finish))
                    if (!Nodes.TryGetValue($"{new Vector2Int(finishId.x - 1, finishId.y)}", out finish))
                        if (!Nodes.TryGetValue($"{new Vector2Int(finishId.x, finishId.y - 1)}", out finish))
                            return null;
        start.SetDistance(finish);
        var activeTiles = new List<Node>() { start };
        var checkedIds = new List<string>();
        var next = start;
        while (next.Position.x != finish.Position.x ||
            next.Position.y != finish.Position.y)
        {
            try
            {
                next = activeTiles[0];
            }
            catch (System.Exception)
            {
                return new List<Vector2Int>();
            }
            for (int i = 1; i < activeTiles.Count; i++)
            {
                if (activeTiles[i].HoristicMoveCost < next.HoristicMoveCost) 
                    next = activeTiles[i];
            }
            var rel = relatedIds(next.Position, checkedIds);
            for (int i = 0; i < rel.Count; i++)
            {
                Nodes[rel[i]].SetDistance(finish);
                Nodes[rel[i]].MoveCost = next.MoveCost + 1;
                Nodes[rel[i]].Parent = next;
                activeTiles.Add(Nodes[rel[i]]);
            }
            checkedIds.Add($"{next.Position}");
            activeTiles.Remove(next);
        }
        var positions = new List<Vector2Int>();
        try
        {
            do
            {
                positions.Insert(0,next.Position);
                next = next.Parent;
            } while (next.Parent != null);
        }
        catch (System.Exception)
        {
            return new List<Vector2Int>() { startId };
        }
        return positions;
    }
    public List<Node> SelectRandomAvailableNodes()
    {
        var lst = new List<Node>();
        for (int i = 0; i < 3; i++)
        {
            Node node;
            do
            {
                var nom = new Vector2Int(Random.Range(-20, 20), Random.Range(-20, 20));
                Nodes.TryGetValue($"{nom}", out node);
            } while (node == null);
            lst.Add(node);
        }
        return lst;
    }
    private List<string> relatedIds(Vector2Int position, List<string> checkedIds)
    {
        var lst = new List<string>();
        if (Nodes.ContainsKey($"{new Vector2Int(position.x + 1, position.y)}") && !checkedIds.Contains($"{new Vector2Int(position.x + 1, position.y)}"))
            lst.Add($"{new Vector2Int(position.x + 1, position.y)}");
        if (Nodes.ContainsKey($"{new Vector2Int(position.x, position.y + 1)}") && !checkedIds.Contains($"{new Vector2Int(position.x, position.y + 1)}"))
            lst.Add($"{new Vector2Int(position.x, position.y + 1)}");
        if (Nodes.ContainsKey($"{new Vector2Int(position.x - 1, position.y)}") && !checkedIds.Contains($"{new Vector2Int(position.x - 1, position.y)}"))
            lst.Add($"{new Vector2Int(position.x - 1, position.y)}");
        if (Nodes.ContainsKey($"{new Vector2Int(position.x, position.y - 1)}") && !checkedIds.Contains($"{new Vector2Int(position.x, position.y - 1)}"))
            lst.Add($"{new Vector2Int(position.x, position.y - 1)}");
        return lst;
    }
}
