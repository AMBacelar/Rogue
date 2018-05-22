using UnityEngine;
using System.Collections.Generic;
using RoyT.AStar;

public class EnemyActor : Actor
{
	BoardPosition targetPos;
	BoardPosition position;

	void Awake()
	{
		targetPos = GameObject.FindGameObjectWithTag("Player").GetComponent<BoardPosition>();
		position = gameObject.GetComponentInParent<BoardPosition>();
	}
	public override Action GetAction()
	{
		Position[] solution = BoardManager.instance.grid.GetPath(new Position(position.X, position.Y), new Position(targetPos.X, targetPos.Y), MovementPatterns.Hexagonal, 50);

		if (solution.Length > 1)
		{
			Position node = solution[1];
			IntVector2 direction = IntVector2.GetDirection(node.X - gameObject.GetComponentInParent<BoardPosition>().X, node.Y - gameObject.GetComponentInParent<BoardPosition>().Y);
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