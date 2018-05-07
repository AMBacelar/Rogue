using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyActor : Actor
{
    public List<Node> CurrentPath = new List<Node>();

    public override Action GetAction()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        IEnumerator pathfinder = Astar.GeneratePath(gameObject.GetComponentInParent<BoardPosition>(), target.GetComponent<BoardPosition>(), this);
        StartCoroutine(pathfinder);
        return GetComponent<RestAction>();
    }
}