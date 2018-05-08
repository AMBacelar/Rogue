using System.Collections;
using System.Collections.Generic;

public static class MapUtilityMethods
{
    public static bool IsOutOfBounds(Hex[,] mapIn, int x, int y)
    {
        if (x < 0 || y < 0)
        {
            return true;
        }
        else if (x > mapIn.GetLength(0) - 1 || y > mapIn.GetLength(1) - 1)
        {
            return true;
        }
        return false;
    }

    public static bool IsWall(Hex[,] mapIn, int x, int y)
    {
        // Consider out-of-bound a wall
        if (IsOutOfBounds(mapIn, x, y))
        {
            return true;
        }

        if (mapIn[x, y].isWalkable == false)
        {
            return true;
        }

        if (mapIn[x, y].isWalkable == true)
        {
            return false;
        }
        return false;
    }

    public static int Heuristics(Hex a, Hex b)
    {
        return Hex.Distance(a, b);
    }
}
