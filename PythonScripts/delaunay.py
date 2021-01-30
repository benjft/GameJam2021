#!/python3.9
import random
import tkinter as tk

def _merge(left_nodes: list[tuple[int, int]],
           left_edges: list[tuple[int, int]],
           right_nodes: list[tuple[int, int]],
           right_edges: list[tuple[int, int]]) -> list[tuple[int, int]]:




def _get_edges(nodes: list[tuple[int, tuple[int, int]]]) -> list[tuple[int, int]]:
    """
    Form the triangulation through divide and conquer.
    Assumes that nodes are sorted by x coordinate
    """
    n = len(nodes)
    if n <= 3:
        if n == 1:
            raise ValueError("n = 1. This should never happen")
        # generate edges
        edges = [(nodes[i][0], nodes[j][0]) for i in range(n) for j in range(i+1, n)]
        return edges

    left_nodes = nodes[:n//2]
    right_nodes = nodes[n//2:]

    left_edges = _get_edges(left_nodes)
    right_edges = _get_edges(right_nodes)

    # TODO: link left and right
    return left_edges + right_edges


def get_edges(nodes: list[tuple[int, int]]) -> list[tuple[int, int]]:
    """
    Takes a list of node (x,y) coordinates and returns a list of edges
    (from, to) linking pairs of nodes.
    """

    # sort the list by x coordinate, keep original index as reference.
    indexed_nodes = ((k, v) for k, v in enumerate(nodes))
    indexed_nodes = sorted(indexed_nodes, key=lambda v: v[1])

    return _get_edges(indexed_nodes)


def draw_graph(nodes: list[tuple[int, int]], edges: list[tuple[int, int]]):
    root = tk.Tk()

    canvas = tk.Canvas(root, bg="black", height=10+max(y for x, y in nodes), width=10+max(x for x, y in nodes))
    canvas.pack()
    for x, y in nodes:
        canvas.create_oval(x, y, x+10, y+10, fill="white")

    for n1, n2 in edges:
        x1, y1 = nodes[n1]
        x2, y2 = nodes[n2]

        canvas.create_line(x1+5, y1+5, x2+5, y2+5, fill="white", width=3)

    root.mainloop()


if __name__ == '__main__':
    rand_coord = lambda: 10*random.randint(0, 30)
    nodes = [(rand_coord(), rand_coord()) for i in range(10)]
    edges = get_edges(nodes)
    draw_graph(nodes, edges)
