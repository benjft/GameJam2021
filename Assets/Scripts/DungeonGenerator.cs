using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour {
    public int roomRadius = 10;
    public int coridoorRadius = 1;

    char[,] buildMap(List<LayoutNode> nodes) {
        
        var minX = nodes.Min(n => n.X - 2*roomRadius);
        var minY = nodes.Min(n => n.Y - 2*roomRadius);

        var maxX = nodes.Max(n => n.X + 2*roomRadius);
        var maxY = nodes.Max(n => n.Y + 2*roomRadius);

        var map = new char[1 + maxX - minX, 1 + maxY - minY];
        foreach (var node in nodes) {
            for (var x = -roomRadius; x <= roomRadius; x++) {
                for (var y = -roomRadius; y <= roomRadius; y++) {
                    var r2 = x * x + y * y;
                    if (r2 > roomRadius * roomRadius)
                        continue;
                    map[node.X + x - minX, node.Y + y - minY] = 'f';
                }
            }

            foreach (var nebr in node.neighbors) {
                var dx = (double) (nebr.X - node.X);
                var dy = (double) (nebr.Y - node.Y);

                if (dx < Math.Abs(dy)) {
                    var slope = dx / dy;
                    for (var yProg = 0; yProg < dy; yProg++) {
                        int x = node.X + (int) (yProg * slope);
                        int y = node.Y + yProg;
                        for (var r = -coridoorRadius; r <= coridoorRadius; r++) {
                            map[x + r - minX, y - minY] = 'f';
                        }
                    }
                } else {
                    var slope = dy / dx;
                    for (var xProg = 0; xProg < dx; xProg++) {
                        int y = node.Y + (int) (xProg * slope);
                        int x = node.X + xProg;
                        for (var r = -coridoorRadius; r <= coridoorRadius; r++) {
                            map[x - minX, y + r - minY] = 'f';
                        }
                    }
                }
            }
        }

        var startNode = nodes.First();
        map[startNode.X - minX, startNode.Y - minY] = 's';
        var endNode = nodes.Last();
        map[endNode.X - minX, endNode.Y - minY] = 'e';
        return map;
    }
    
    // Start is called before the first frame update
    void Start() {
        var layout = new Layout();
        layout.NRooms = 20;
        layout.RoomSize = 2 * roomRadius;
        layout.CorridoorSize = coridoorRadius;
        layout.MAXCorridoorLength = 3 * (roomRadius + coridoorRadius);
        layout.Generate();
        
        var map = buildMap(layout.LayoutNodes);

        var tileWall = Resources.Load<GameObject>("Tiles/TileWall");
        var tileFloor = Resources.Load<GameObject>("Tiles/TileFloor");
        tileFloor.tag = "Floor";
        var tileStart = Resources.Load<GameObject>("Tiles/TileStart");
        tileStart.tag = "Start";
        var tileEnd = Resources.Load<GameObject>("Tiles/TileEnd");
        tileEnd.tag = "End";

        var cols = map.GetLength(0);
        var rows = map.GetLength(1);
        var scale = 1f;
        
        for (var col = 0; col < cols; col++) {
            for (var row = 0; row < rows; row++) {
                var posX = col * scale;
                var posY = -row * scale;
                GameObject tile;
                switch (map[col, row]) {
                    case 's': tile = Instantiate(tileStart, transform);
                        break;
                    case 'e': tile = Instantiate(tileEnd, transform);
                        break;
                    case 'f': tile = Instantiate(tileFloor, transform);
                        break;
                    default: tile = Instantiate(tileWall, transform);
                        break;
                }
                tile.transform.position = new Vector3(posX, posY);
            }
        }

        var gridW = cols * scale;
        var gridH = rows * scale;
        transform.position = new Vector3(-gridW / 2, gridH / 2);
        GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.FindGameObjectWithTag("Start").transform.position;
    }
}


internal class Layout {
    internal int NRooms = 10;
    internal int RoomSize = 20;
    internal int CorridoorSize = 10;
    private double ExcludeNear => Math.Pow(RoomSize + CorridoorSize, 2);
    internal int MAXCorridoorLength = 40;
    private double ExculdeFar => Math.Pow(RoomSize + MAXCorridoorLength, 2);

    internal int Side => (int)((RoomSize + MAXCorridoorLength) * Math.Sqrt(NRooms));
    internal int Seed = Random.Range(0, int.MaxValue);

    internal List<LayoutNode> LayoutNodes;
    
    private double GetAngle(LayoutNode a, LayoutNode b) {
        return Math.Atan2(b.Y - a.Y, b.X - a.X);
    }

    private double GetInteriorAngle(LayoutNode a, LayoutNode b, LayoutNode c) {
        var ab = GetAngle(a, b);
        var ac = GetAngle(a, c);
        var angle = ac - ab;
        if (angle <= -Math.PI) {
            angle += 2 * Math.PI;
        } else if (angle > Math.PI) {
            angle -= 2 * Math.PI;
        }
        return angle;
    }

    private LayoutNode ExpandRight(LayoutNode lowLeft, LayoutNode lowRight) {
        var moved = true;
        while (moved) {
            moved = false;
            var angle = GetAngle(lowLeft, lowRight);
            var candidates = lowRight.neighbors
                .Select(n => new Tuple<LayoutNode, double>(n, GetAngle(lowLeft, n)))
                .Where(t => t.Item2 < angle)
                .OrderBy(t => t.Item2)
                .Select(t => t.Item1)
                .ToList();
            if (candidates.Count() != 0) {
                lowRight = candidates.First();
                moved = true;
            }
        }
        return lowRight;
    }

    private LayoutNode ExpandLeft(LayoutNode lowLeft, LayoutNode lowRight) {
        var moved = true;
        while (moved) {
            moved = false;
            var angle = GetAngle(lowLeft, lowRight);
            var candidates = lowLeft.neighbors
                .Select(n => new Tuple<LayoutNode, double>(n, GetAngle(n, lowRight)))
                .Where(t => t.Item2 > angle)
                .OrderByDescending(t => t.Item2)
                .Select(t => t.Item1)
                .ToList();
            if (candidates.Count() != 0) {
                lowLeft = candidates.First();
                moved = true;
            }
        }
        return lowLeft;
    }
    
    private Tuple<LayoutNode, LayoutNode> FindBottom(HashSet<LayoutNode> left, HashSet<LayoutNode> right) {
        LayoutNode lowLeft = null,
                   lowRight = null;
        foreach (var node in left) {
            if (lowLeft == null || lowLeft.Y > node.Y) {
                lowLeft = node;
            } 
        }
        foreach (var node in right) {
            if (lowRight == null || lowRight.Y > node.Y) {
                lowRight = node;
            } 
        }

        if (lowLeft.Y < lowRight.Y) {
            lowRight = ExpandRight(lowLeft, lowRight);
        }
        var changed = true;
        while (changed) {
            var newLow = ExpandLeft(lowLeft, lowRight);
            changed = newLow != lowLeft;
            lowLeft = newLow;
            newLow = ExpandRight(lowLeft, lowRight);
            changed = changed || lowRight != newLow;
            lowRight = newLow;
        }

        return new Tuple<LayoutNode, LayoutNode>(lowLeft, lowRight);
    }
    
    private List<LayoutNode> LeftCandidates(LayoutNode lowLeft, LayoutNode lowRight, HashSet<LayoutNode> right) {
        return lowLeft.neighbors
            .Where(n => !right.Contains(n))
            .Select(n => new Tuple<LayoutNode, double>(n, GetInteriorAngle(lowLeft, lowRight, n)))
            .Where(t => t.Item2 >= 0)
            .OrderBy(t => t.Item2)
            .Select(t => t.Item1)
            .ToList();
    }

    private List<LayoutNode> RightCandidates(LayoutNode lowLeft, LayoutNode lowRight, HashSet<LayoutNode> left) {
        return lowRight.neighbors
            .Where(n => !left.Contains(n))
            .Select(n => new Tuple<LayoutNode, double>(n, GetInteriorAngle(lowRight, lowLeft, n)))
            .Where(t => t.Item2 <= 0)
            .OrderByDescending(t => t.Item2)
            .Select(t => t.Item1)
            .ToList();
    }
    
    private Tuple<double, double, double> GetCCircle(LayoutNode a, LayoutNode b, LayoutNode c) {
        var midAB = new Vector2(a.X + b.X, a.Y + b.Y) / 2;
        var midAC = new Vector2(a.X + c.X, a.Y + c.Y) / 2;

        double dxAB = a.X - b.X,
               dyAB = a.Y - b.Y,
               perpAB = -dxAB / dyAB,
               offAB = midAB.y - perpAB * midAB.x,
               dxAC = a.X - c.X,
               dyAC = a.Y - c.Y,
               perpAC = -dxAC / dyAC,
               offAC = midAC.y - perpAC*midAC.x,
               cx = (offAC - offAB) / (perpAB - perpAC),
               cy = perpAB * cx + offAB,
               r2 = Math.Pow(cx - a.X, 2) + Math.Pow(cy - a.Y, 2);
        return new Tuple<double, double, double>(cx, cy, r2);
    }
    
    private bool TryAdd(LayoutNode candidate, LayoutNode corner, LayoutNode linkWith, HashSet<LayoutNode> nodes) {
        var (circleX, circleY, circleR2) = GetCCircle(candidate, corner, linkWith);
        return nodes.Where(n => n != candidate && n != corner && n != linkWith)
            .Select(n => Math.Pow(circleX - n.X, 2) + Math.Pow(circleY - n.Y, 2))
            .All(nodeR2 => nodeR2 > circleR2);
    }

    private LayoutNode TryFirst(
        ICollection<LayoutNode> candidates, LayoutNode corner, LayoutNode linkWith, HashSet<LayoutNode> nodes,
        ICollection<LayoutNode> reject) {
        if (candidates.Count <= 0) {
            return null;
        }

        var candidate = candidates.First();
        candidates.Remove(candidate);
        if (!TryAdd(candidate, corner, linkWith, nodes)) {
            reject.Add(candidate);
            return null;
        }

        foreach (var node in reject) {
            node.neighbors.Remove(corner);
            corner.neighbors.Remove(node);
        }
        candidate.neighbors.Add(linkWith);
        linkWith.neighbors.Add(candidate);
                
        return candidate;
    }

    private void Merge(HashSet<LayoutNode> left, HashSet<LayoutNode> right) {
        var (lowLeft, lowRight) = FindBottom(left, right);

        lowLeft.neighbors.Add(lowRight);
        lowRight.neighbors.Add(lowLeft);
        
        var rejectLeft = new HashSet<LayoutNode>();
        var rejectRight = new HashSet<LayoutNode>();
        var leftCandidates = LeftCandidates(lowLeft, lowRight, right);
        var rightCandidates = RightCandidates(lowLeft, lowRight, left);

        void Clear() {
                rejectLeft.Clear();
                rejectRight.Clear();
                leftCandidates = LeftCandidates(lowLeft, lowRight, right);
                rightCandidates = RightCandidates(lowLeft, lowRight, left);
        }
        
        var allNodes = new HashSet<LayoutNode>(left.Union(right));
        while (leftCandidates.Count > 0 || rightCandidates.Count > 0) {
            var candidate = TryFirst(leftCandidates, lowLeft, lowRight, allNodes, rejectLeft);
            if (candidate != null) {
                lowLeft = candidate;
                Clear();
            }
            candidate = TryFirst(rightCandidates, lowRight, lowLeft, allNodes, rejectRight);
            if (candidate != null) {
                lowRight = candidate;
                Clear();
            }
        }
    }
    
    private void CreateGraph(List<LayoutNode> sortedNodes) {
        if (sortedNodes.Count <= 3) {
            foreach (var node1 in sortedNodes) {
                foreach (var node2 in sortedNodes.Where(node2 => node1 != node2)) {
                    node1.neighbors.Add(node2);
                }
            }
            return;
        }

        var left = sortedNodes.GetRange(0, sortedNodes.Count / 2);
        CreateGraph(left);
        
        var right = sortedNodes.GetRange(
            sortedNodes.Count / 2,
            sortedNodes.Count - sortedNodes.Count / 2);
        CreateGraph(right);
        
        Merge(new HashSet<LayoutNode>(left), new HashSet<LayoutNode>(right));
    }
    

    internal void Generate() {
        Debug.Log(Seed);
        Random.InitState(Seed);
        LayoutNodes = new List<LayoutNode>();
        // i to prevent stuck condition if room can't be placed.
        for (var i = 0; LayoutNodes.Count < NRooms && i < 100; i++) {
            // new coord
            var nodePos = new Vector2Int(Random.Range(0, Side), Random.Range(0, Side));
            var distance = LayoutNodes.Select(node => (node.Position - nodePos).sqrMagnitude).ToList();
            // are we within exculsion zone
            var near = distance.Any(d=> d < ExcludeNear);
            // are we too far away
            var far = distance.All(d => d > ExculdeFar) && distance.Count > 0;
            
            // if we are neither too far or too near, add to list
            if (!near && !far) {
                LayoutNodes.Add(new LayoutNode(nodePos));
                i = 0; // reset loop counter
            }
        }

        LayoutNodes = LayoutNodes.OrderBy(n => n.Position.x).ThenBy(n => n.Position.y).ToList();
        CreateGraph(LayoutNodes);
        foreach (var node in LayoutNodes) {
            var farNodes = node.neighbors.Where(n => (n.Position - node.Position).sqrMagnitude > 2*ExculdeFar).ToList();
            foreach (var farNode in farNodes) {
                node.neighbors.Remove(farNode);
                farNode.neighbors.Remove(node);
            }

            if (node.neighbors.Count == 0) {
                throw new Exception("Node is completely disconnected?!");
            }
        }
            
    }
}

internal class LayoutNode {
    internal Vector2Int Position;
    internal int X => Position.x;
    internal int Y => Position.y;
    internal List<LayoutNode> neighbors = new List<LayoutNode>();
    internal LayoutNode(Vector2Int pos) {
        this.Position = pos;
    }
}