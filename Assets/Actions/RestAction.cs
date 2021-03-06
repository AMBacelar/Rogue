﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class RestAction : Action
{
    public int energyGain = 1;

    override public ActionResult Perform()
    {
        GetComponent<Actor>().GainEnergy(energyGain);
        state = ActionState.FINISHED;
        return ActionResult.SUCCESS;
    }
}