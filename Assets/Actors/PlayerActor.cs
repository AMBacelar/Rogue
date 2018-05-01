
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AttackAction))]
public class PlayerActor : Actor
{

    public override Action GetAction()
    {
        if (!HasEnergyToActivate(GetComponent<AttackAction>().EnergyCost))
        {
            return GetComponent<RestAction>();
        }
        if (SwipeManager.Instance.IsSwiping(SwipeDirection.Left | SwipeDirection.Up))
        {
            return GetAttackAction(IntVector2.NorthWest);
        }
        if (SwipeManager.Instance.IsSwiping(SwipeDirection.Left))
        {
            return GetAttackAction(IntVector2.West);
        }
        if (SwipeManager.Instance.IsSwiping(SwipeDirection.Left | SwipeDirection.Down))
        {
            return GetAttackAction(IntVector2.SouthWest);
        }
        if (SwipeManager.Instance.IsSwiping(SwipeDirection.Right | SwipeDirection.Down))
        {
            return GetAttackAction(IntVector2.SouthEast);
        }
        if (SwipeManager.Instance.IsSwiping(SwipeDirection.Right))
        {
            return GetAttackAction(IntVector2.East);
        }
        if (SwipeManager.Instance.IsSwiping(SwipeDirection.Right | SwipeDirection.Up))
        {
            return GetAttackAction(IntVector2.NorthEast);
        }
        return null;
    }

    private Action GetAttackAction(IntVector2 direction)
    {
        AttackAction action = GetComponent<AttackAction>();
        action.direction = direction;
        return action;
    }

}