using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RestAction))]
public class MoveAction : Action
{

	public IntVector2 direction;

	override public ActionResult Perform()
	{
		if (!GetComponent<Actor>().HasEnergyToActivate(EnergyCost))
		{
			return ActionResult.FAILURE(GetComponent<RestAction>());
		}
		if (!CanPerform())
		{
			return ActionResult.FAILURE(GetComponent<RestAction>());
		}
		state = ActionState.EXECUTING;
		return ActionResult.SUCCESS;

	}

	void Update()
	{
		if (state == ActionState.EXECUTING)
		{
			BoardPosition boardPosition = GetComponent<BoardPosition>();
			boardPosition.MoveDirection(direction);
			//Temporary movement.

            StartCoroutine(smooth_move(BoardManager.instance.TileCoordToWorldCoord(direction.X, direction.Y), 0.1f)); //Calling the coroutine.

			state = ActionState.FINISHED;
		}
	}

    IEnumerator smooth_move(Vector3 direction, float time)
    {
        float elapsedTime = 0;
        Vector3 startingPos = transform.position; //Starting position.
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, (startingPos + direction), (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

	private bool CanPerform()
	{
		return GetComponent<BoardPosition>().CanMoveDirection(direction);
	}
}