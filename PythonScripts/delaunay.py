#!/python3.9
import random
import tkinter as tk


def _merge(all_nodes: dict[int, tuple[int, int]],
           left_nodes: list[int],
           left_edges: list[tuple[int, int]],
           right_nodes: list[int],
           right_edges: list[tuple[int, int]]) -> list[tuple[int, int]]:
    edge_map: dict[int, list[int]] = {}
    for a, b in left_edges + right_edges:
        edge_map[a] = edge_map.get(a, []) + [b]
        edge_map[b] = edge_map.get(b, []) + [a]

    bottom_left = min(left_nodes, key=lambda i: (all_nodes[i][1], -all_nodes[i][0]))
    bottom_right = min(right_nodes, key=lambda i: (all_nodes[i][1], all_nodes[i][0]))

    link_edges: list[tuple[int, int]] = [(bottom_left, bottom_right)]

    left_candidates = [node for node in edge_map[bottom_left] if all_nodes[node][1] > all_nodes[bottom_left][1]]
    right_candidates = [node for node in edge_map[bottom_right] if all_nodes[node][1] > all_nodes[bottom_right][1]]

    left_candidate = max(left_candidates, key=lambda i: all_nodes[i][0])
    right_candidate = min(right_candidates, key=lambda i: all_nodes[i][0])

    return ...

def _get_edges(all_nodes: dict[int, tuple[int, int]], x_order: list[int]) -> list[tuple[int, int]]:
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

    return left_edges + right_edges # _merge(all_nodes, left_nodes, left_edges, right_nodes, right_edges)


def get_edges(nodes: list[tuple[int, int]]) -> list[tuple[int, int]]:
    """
    Takes a list of node (x,y) coordinates and returns a list of edges
    (from, to) linking pairs of nodes.
    """

    # sort the list by x coordinate, keep original index as reference.
    node_dict = {k: v for k, v in enumerate(nodes)}
    nodes_by_x = sorted(node_dict.keys(), key=lambda k: node_dict[k])
    return _get_edges(node_dict, nodes_by_x)


def draw_graph(nodes: list[tuple[int, int]], edges: list[tuple[int, int]]):
    root = tk.Tk()

    canvas = tk.Canvas(root, bg="black", height=10 + max(y for x, y in nodes), width=10 + max(x for x, y in nodes))
    canvas.pack()
    for x, y in nodes:
        canvas.create_oval(x, y, x + 10, y + 10, fill="white")

    for n1, n2 in edges:
        x1, y1 = nodes[n1]
        x2, y2 = nodes[n2]

        canvas.create_line(x1 + 5, y1 + 5, x2 + 5, y2 + 5, fill="white", width=3)

    root.mainloop()


def main():
    rand_coord = lambda: 10 * random.randint(0, 30)
    nodes = [(rand_coord(), rand_coord()) for i in range(10)]
    edges = get_edges(nodes)
    draw_graph(nodes, edges)


if __name__ == '__main__':
    main()
