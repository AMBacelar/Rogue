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

            //transform.Translate(BoardManager.instance.TileCoordToWorldCoord(direction.X, direction.Y));
            StartCoroutine(smooth_move(direction, 0.1f)); //Calling the coroutine.

			state = ActionState.FINISHED;
		}
	}

    IEnumerator smooth_move(IntVector2 offset, float time)
    {
        float elapsedTime = 0;
        IntVector2 boardPosition = GetComponent<BoardPosition>().Position;
        IntVector2 targetPosition = boardPosition + offset;
        Debug.Log($"adding our board position: {boardPosition.X},{boardPosition.Y} with the offset: {offset.X}, {offset.Y}... to get: {targetPosition.X}, {targetPosition.Y}");
        Vector3 startingPos = transform.position; //Starting position.
        Vector3 targetPos = BoardManager.instance.TileCoordToWorldCoord(targetPosition.X, targetPosition.Y);

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, targetPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

	private bool CanPerform()
	{
		return GetComponent<BoardPosition>().CanMoveDirection(direction);
	}
}