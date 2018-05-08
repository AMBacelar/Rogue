using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Astar
{

	public static Node[,] GenerateGraph(Hex[,] mapIn)
	{
		int x = mapIn.GetLength(0);
		int y = mapIn.GetLength(1);
		Node[,] graph = new Node[x, y];

		// populate the graph
		foreach (Hex hex in mapIn)
		{
			graph[hex.Q, hex.R] = new Node(hex.Q, hex.R);
		}
		// initialize the neighbours for each node
		foreach (Hex hex in mapIn)
		{
			Hex[] neighbours = hex.GetNeighbours();
			foreach (Hex neighbour in neighbours)
			{
				if (BoardManager.instance.IsWithinBounds(neighbour.Q, neighbour.R))
				{
					graph[hex.Q, hex.R].neighbours.Add(graph[neighbour.Q, neighbour.R]);
				}
			}
		}
		return graph;
	}

	public static float CostToEnterTile(int targetX, int targetY)
	{
		Hex hex = BoardManager.instance.GetHex(targetX, targetY);

		if (hex.isWalkable == false)
			return Mathf.Infinity;

		float cost = BoardManager.instance.tileTypes[hex.tileType].movementCost;

		return cost;

	}

	#region Pathfinding

	public static List<Node> GeneratePath(BoardPosition from, BoardPosition to, Node[,] graph)
	{
		Dictionary<Node, float> dist = new Dictionary<Node, float>();
		Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

		List<Node> unvisited = new List<Node>();

		Node source = graph[from.X, from.Y];
		Node target = graph[to.X, to.Y];

		dist[source] = 0;
		prev[source] = null;

		foreach (Node v in graph)
		{
			// TODO Consider replacing this part with a flood fill, right now it's unnecesary, because every tile is reachable
			if (v != source)
			{
				dist[v] = Mathf.Infinity;
				prev[v] = null;
			}
			unvisited.Add(v);
		}

		while (unvisited.Count > 0)
		{
			Node u = null;

			foreach (Node possibleU in unvisited)
			{
				if (u == null || dist[possibleU] < dist[u])
				{
					u = possibleU;
				}
			}

			if (u == target)
			{
				break;
			}

			unvisited.Remove(u);

			foreach (Node v in u.neighbours)
			{
				float alt = dist[u] + CostToEnterTile(v.x, v.y) + BoardManager.instance.Heuristics(v, target);
				if (alt < dist[v])
				{
					dist[v] = alt;
					prev[v] = u;
				}

			}
		}
		if (prev[target] == null)
		{
			Debug.Log("No Path Found");
			return null;
		}
		List<Node> currentPath = new List<Node>();

		Node curr = target;

		while (curr != null)
		{
			currentPath.Add(curr);
			curr = prev[curr];
		}

		currentPath.Reverse();

		return currentPath;
	}
	#endregion

}
