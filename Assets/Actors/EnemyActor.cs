using UnityEngine;
using System.Collections;

public class EnemyActor : Actor
{
    public override Action GetAction()
    {
        return GetComponent<RestAction>();
    }
}