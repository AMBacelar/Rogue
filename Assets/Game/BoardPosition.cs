using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPosition : MonoBehaviour
{

    [SerializeField]
    private IntVector2 _position;
    public IntVector2 Position
    {
        get { return _position; }
    }

    public int X
    {
        get { return _position.X; }
    }

    public int Y
    {
        get { return _position.Y; }
    }

    // Use this for initialization
    void Start()
    {
        BoardManager.instance.RegisterDynamicBoardPosition(this);
    }

    public void TeleportTo(int x, int y)
    {
        BoardManager.instance.UnregisterDynamicBoardPosition(this);
        _position = new IntVector2(x, y);
        BoardManager.instance.RegisterDynamicBoardPosition(this);
    }

    public void MoveDirection(IntVector2 direction)
    {
        if (!CanMoveDirection(direction))
        {
            Debug.LogError("Cannot ModeDirection");
            return;
        }
        TeleportTo(X + direction.X, Y + direction.Y);
    }

    public bool CanMoveDirection(IntVector2 direction)
    {
        return BoardManager.instance.IsPassable(Position + direction);
    }

    public BoardPosition GetAdjacent(IntVector2 direction)
    {
        return BoardManager.instance.GetOccupied(Position + direction);
    }

    public int ManhattanDistance(BoardPosition to)
    {
        return Mathf.Abs(X - to.X) + Mathf.Abs(Y - to.Y);
    }

    public bool IsAdjacent(BoardPosition to)
    {
        return ManhattanDistance(to) == 1;
    }

    public void Unregister()
    {
        BoardManager.instance.UnregisterDynamicBoardPosition(this);
    }

}
