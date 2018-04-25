
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
        if (Input.GetKeyDown(KeyCode.W))
        {
            return GetAttackAction(IntVector2.NorthWest);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            return GetAttackAction(IntVector2.West);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            return GetAttackAction(IntVector2.SouthWest);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            return GetAttackAction(IntVector2.SouthEast);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            return GetAttackAction(IntVector2.East);
        }
        if (Input.GetKeyDown(KeyCode.E))
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