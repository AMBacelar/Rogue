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

    void Start()
    {
        Register();
    }

    public void Initialize(IntVector2 pos)
    {
        _position = pos;
    }

    public void Initialize(int x, int y)
    {
        Initialize(new IntVector2(x, y));
    }

    public void TeleportTo(int x, int y)
    {
        Unregister();
        _position = new IntVector2(x, y);
        Register();
    }

    public void TeleportTo(IntVector2 pos)
    {
        TeleportTo(pos.X, pos.Y);
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

    public void Register()
    {
        BoardManager.instance.RegisterDynamicBoardPosition(this);
    }

    public void Unregister()
    {
        BoardManager.instance.UnregisterDynamicBoardPosition(this);
    }

}
