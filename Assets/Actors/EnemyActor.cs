using UnityEngine;
using System.Collections.Generic;

public class EnemyActor : Actor
{
	GameObject target;
	BoardPosition targetPos;
	BoardPosition position;

	List<Node> solution = new List<Node>();
	void Awake()
	{
		target = GameObject.FindGameObjectWithTag("Player");
		targetPos = target.GetComponent<BoardPosition>();
		position = gameObject.GetComponentInParent<BoardPosition>();
	}
	public override Action GetAction()
	{
		solution = Astar.GeneratePath(position, targetPos, BoardManager.instance.graph);

		if (solution.Count > 1)
		{
			Node node = solution[1];
			IntVector2 direction = IntVector2.GetDirection(node.x - gameObject.GetComponentInParent<BoardPosition>().X, node.y - gameObject.GetComponentInParent<BoardPosition>().Y);
			return GetAttackAction(direction);
		}
		return GetComponent<RestAction>();
	}

	protected Action GetAttackAction(IntVector2 direction)
	{
		AttackAction action = GetComponent<AttackAction>();
		action.direction = direction;
		return action;
	}
}