using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public int EnergyCost = 0;

    public enum ActionState { IDLE, EXECUTING, FINISHED };
    public ActionState state = ActionState.IDLE;

    abstract public ActionResult Perform();
}