using UnityEngine;
using System.Collections.Generic;

public class Node
{
	public List<Node> neighbours;
	readonly int _x;
	readonly int _y;
	public int x { get { return _x; } }
	public int y { get { return _y; } }

	public Node(int x, int y)
	{
		_x = x;
		_y = y;
		neighbours = new List<Node>();
	}

	public float DistanceTo(Node n)
	{
		if (n == null)
		{
			Debug.LogError("WTF?");
		}

		return Vector2.Distance(
			new Vector2(x, y),
			new Vector2(n.x, n.y)
		);
	}

	#region Overrides

	//public static bool operator ==(Node a, Node b)
	//{
	//	if (a.x == b.x && a.y == b.y) Debug.Log("Is " + a + " the same as " + b);
	//	return a.x == b.x && a.y == b.y;
	//}

	//public static bool operator !=(Node a, Node b)
	//{
	//	return !(a == b);
	//}

	public override string ToString()
	{
		return "Node - x: " + x + " y: " + y;
	}

	#endregion
}