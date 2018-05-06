using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Astar
{

    public static Node[,] generateGraph(Hex[,] mapIn)
    {
        int x = mapIn.GetLength(0);
        int y = mapIn.GetLength(1);
        Node[,] graph = new Node[x, y];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                graph[x, y] = new Node(x, y);
            }
        }

        return graph;
    }

}
