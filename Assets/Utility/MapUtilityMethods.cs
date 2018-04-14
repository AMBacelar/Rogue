using System.Collections;
using System.Collections.Generic;

public static class MapUtilityMethods
{
    public static bool IsOutOfBounds(int[,] mapIn, int x, int y)
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

    public static bool IsWall(int[,] mapIn, int x, int y)
    {
        // Consider out-of-bound a wall
        if (IsOutOfBounds(mapIn, x, y))
        {
            return true;
        }

        if (mapIn[x, y] == 1)
        {
            return true;
        }

        if (mapIn[x, y] == 0)
        {
            return false;
        }
        return false;
    }
}
