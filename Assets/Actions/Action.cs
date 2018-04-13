using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{

    [SerializeField]
    private int _energyCost;
    public int EnergyCost
    {
        get { return _energyCost; }
    }

    public enum ActionState { IDLE, EXECUTING, FINISHED };
    public ActionState state = ActionState.IDLE;

    abstract public ActionResult Perform();
}