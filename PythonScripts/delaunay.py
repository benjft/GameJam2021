#!/python3.9
import math
import random
import tkinter as tk
from typing import Optional


def get_slope(a: tuple[int, int], b: tuple[int, int]) -> float:
    return (a[1] - b[1]) / (a[0] - b[0])


def get_mid(a: tuple[int, int], b: tuple[int, int]) -> tuple[float, float]:
    x = (a[0] + b[0]) / 2
    y = (a[1] + b[1]) / 2

    return x, y


def get_ccentre(a: tuple[int, int], b: tuple[int, int], c: tuple[int, int]) -> tuple[float, float, float]:
    mid_ab = get_mid(a, b)
    mid_ac = get_mid(a, c)

    grad_ab = -1/get_slope(a, b)
    grad_ac = -1/get_slope(a, c)

    int_ab = mid_ab[1] - grad_ab*mid_ab[0]
    int_ac = mid_ac[1] - grad_ac*mid_ac[0]

    cx = (int_ac - int_ab) / (grad_ab - grad_ac)
    cy = grad_ab * cx + int_ab
    r2 = (cx - a[0])**2 + (cy - a[1])**2

    return cx, cy, r2


def try_add(candidate: int,
            corner: int,
            links_to: int,
            nodes: list[int],
            node_dict: list[tuple[int, int]]) -> bool:
    cx, cy, r2 = get_ccentre(node_dict[candidate], node_dict[links_to], node_dict[corner])
    for node in nodes:
        if node in {candidate, corner, links_to}:
            continue
        x, y = node_dict[node]
        node_r2 = (x-cx)**2 + (y-cy)**2
        if node_r2 <= r2:
            return False

    return True


def get_angle(a, b) -> float:
    return math.atan2(b[1] - a[1], b[0] - a[0])

def get_interior(a, b, c) -> float:
    ab = get_angle(a, b)
    ac = get_angle(a, c)

    ang = ac - ab
    if ang <= -math.pi:
        ang += 2*math.pi
    elif ang > math.pi:
        ang -= 2*math.pi
    return ang

def find_lowest(left, right, nodes, edges):
    bot_left = min(left, key=lambda i: nodes[i][1])
    bot_right = min(right, key=lambda i: nodes[i][1])

    if nodes[bot_left][1] < nodes[bot_right][1]:
        while True:
            angle = get_angle(nodes[bot_left], nodes[bot_right])
            connected = edges[bot_right]
            angles = [get_angle(nodes[bot_left], nodes[r]) for r in edges[bot_right]]
            connected = [con for con, ang in sorted(zip(connected, angles), key=lambda v: v[1]) if 0 < ang < angle]
            if connected:
                bot_right = connected[0]
                continue
            break
        while True:
            angle = get_angle(nodes[bot_left], nodes[bot_right])
            connected = edges[bot_left]
            angles = [get_angle(nodes[l], nodes[bot_right]) for l in edges[bot_left]]
            connected = [con for con, ang in sorted(zip(connected, angles), key=lambda v: v[1]) if 0 < ang < angle]
            if connected:
                bot_left = connected[0]
                continue
            break
    else:
        while True:
            angle = get_angle(nodes[bot_left], nodes[bot_right])
            connected = edges[bot_right]
            angles = [get_angle(nodes[bot_left], nodes[r]) for r in edges[bot_right]]
            connected = [con for con, ang in sorted(zip(connected, angles), key=lambda v: v[1]) if 0 < ang < angle]
            if connected:
                bot_right = connected[0]
                continue
            break
        while True:
            angle = get_angle(nodes[bot_left], nodes[bot_right])
            connected = edges[bot_left]
            angles = [get_angle(nodes[l], nodes[bot_right]) for l in edges[bot_left]]
            connected = [con for con, ang in sorted(zip(connected, angles), key=lambda v: v[1]) if 0 < ang < angle]
            if connected:
                bot_left = connected[0]
                continue
            break
    return bot_left, bot_right

def _merge(all_nodes: list[tuple[int, int]],
           left_nodes: list[int],
           left_edges: list[tuple[int, int]],
           right_nodes: list[int],
           right_edges: list[tuple[int, int]]) -> list[tuple[int, int]]:
    edge_map: dict[int, set[int]] = {}
    for a, b in left_edges + right_edges:
        edge_map.setdefault(a, set()).add(b)
        edge_map.setdefault(b, set()).add(a)

    draw_graph(all_nodes, [(k, n) for k, l in edge_map.items() for n in l if n > k])
    merge_nodes = left_nodes + right_nodes

    bot_left, bot_right = find_lowest(left_nodes, right_nodes, all_nodes, edge_map)

    edge_map[bot_left].add(bot_right)
    edge_map[bot_right].add(bot_left)

    draw_graph(all_nodes, [(k, n) for k, l in edge_map.items() for n in l if n > k])

    left_candidates = None
    right_candidates = None

    def find_candidates():
        nonlocal left_candidates, right_candidates

        left_candidates = [node for node in edge_map[bot_left] if node not in right_nodes]
        left_angles = [get_interior(all_nodes[bot_left], all_nodes[bot_right], all_nodes[node]) for node in left_candidates]
        lca = [(node, angle) for node, angle in zip(left_candidates, left_angles) if angle >= 0]
        left_candidates = [node for node, _ in sorted(lca, key=lambda v: -v[1])]

        right_candidates = [node for node in edge_map[bot_right] if node not in left_nodes]
        right_angles = [get_interior(all_nodes[bot_right], all_nodes[bot_left], all_nodes[node]) for node in right_candidates]
        rca = [(node, angle) for node, angle in zip(right_candidates, right_angles) if angle <= 0]
        right_candidates = [node for node, _ in sorted(rca, key=lambda v: v[1])]

    find_candidates()

    left_reject = []
    right_reject = []

    while left_candidates or right_candidates:
        if left_candidates:
            left = left_candidates.pop()
            success = try_add(left, bot_left, bot_right, merge_nodes, all_nodes)
            if success:
                for node in left_reject:
                    edge_map[bot_left].remove(node)
                    edge_map[node].remove(bot_left)
                edge_map[left].add(bot_right)
                edge_map[bot_right].add(left)

                draw_graph(all_nodes, [(k, n) for k, l in edge_map.items() for n in l if n > k])
                bot_left = left
                left_reject = []
                right_reject = []

                find_candidates()
            else:
                left_reject.append(left)
        if right_candidates:
            right = right_candidates.pop()
            success = try_add(right, bot_right, bot_left, merge_nodes, all_nodes)
            if success:
                for node in right_reject:
                    edge_map[bot_right].remove(node)
                    edge_map[node].remove(bot_right)
                edge_map[right].add(bot_left)
                edge_map[bot_left].add(right)

                draw_graph(all_nodes, [(k, n) for k, l in edge_map.items() for n in l if n > k])
                bot_right = right

                left_reject = []
                right_reject = []

                find_candidates()
            else:
                right_reject.append(right)
    edges = [(k, n) for k, l in edge_map.items() for n in l if n > k]


    return edges


def _get_edges(all_nodes: list[tuple[int, int]], x_order: list[int]) -> list[tuple[int, int]]:
    """
    Form the triangulation through divide and conquer.
    Assumes that nodes are sorted by x coordinate
    """
    n = len(x_order)
    if n <= 3:
        if n == 1:
            raise ValueError("n = 1. This should never happen")
        # generate edges
        edges = [(x_order[i], x_order[j]) for i in range(n) for j in range(i + 1, n)]
        return edges

    left_nodes = x_order[:n // 2]
    right_nodes = x_order[n // 2:]

    left_edges = _get_edges(all_nodes, left_nodes)
    right_edges = _get_edges(all_nodes, right_nodes)

    return _merge(all_nodes, left_nodes, left_edges, right_nodes, right_edges)


def get_edges(nodes: list[tuple[int, int]]) -> list[tuple[int, int]]:
    """
    Takes a list of node (x,y) coordinates and returns a list of edges
    (from, to) linking pairs of nodes.
    """

    # sort the list by x coordinate, keep original index as reference.
    nodes_by_x = sorted(range(len(nodes)), key=lambda k: nodes[k])
    return _get_edges(nodes, nodes_by_x)


def draw_graph(nodes: list[tuple[int, int]], edges: list[tuple[int, int]]):
    root = tk.Tk()

    canvas = tk.Canvas(root, bg="black", height=20 + max(y for x, y in nodes), width=20 + max(x for x, y in nodes))
    canvas.pack()
    for x, y in nodes:
        canvas.create_oval(x, y, x + 20, y + 20, fill="white")

    for n1, n2 in edges:
        x1, y1 = nodes[n1]
        x2, y2 = nodes[n2]

        canvas.create_line(x1 + 10, y1 + 10, x2 + 10, y2 + 10, fill="white", width=3)

    root.mainloop()


def main():
    seed = random.randint(0,1000000)
    print(seed)
    random.seed(seed)
    rand_coord = lambda: 1000 * random.random()
    nodes: list[tuple[float, float]] = []
    while len(nodes) < 10:
        x, y = rand_coord(), rand_coord()
        for x2, y2 in nodes:
            r2 = (x-x2)**2 + (y-y2)**2
            if r2 < 50**2:
                break
        else:
            nodes.append((x, y))

    edges = get_edges(nodes)
    draw_graph(nodes, edges)


if __name__ == '__main__':
    main()
