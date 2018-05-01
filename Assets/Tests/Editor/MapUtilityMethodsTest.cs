using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MapUtilityMethodsTest
{

    private Hex[,] _initializeMap(Hex[,] mapIn)
    {
        int x = mapIn.GetLength(0);
        int y = mapIn.GetLength(1);
        for (int row = 0; row < y; row++)
        {
            for (int column = 0; column < x; column++)
            {
                // If coordinants lie on the the edge of the map (creates a border)
                if (column == 0)
                {
                    mapIn[column, row] = new Hex(false, 1, column, row);
                }
                else if (row == 0)
                {
                    mapIn[column, row] = new Hex(false, 1, column, row);
                }
                else if (column == x - 1)
                {
                    mapIn[column, row] = new Hex(false, 1, column, row);
                }
                else if (row == y - 1)
                {
                    mapIn[column, row] = new Hex(false, 1, column, row);
                }
                else
                {
                    mapIn[column, row] = new Hex(true, 0, column, row);
                }
            }
        }
        return mapIn;
    }


    [Test]
    public void IsOutOfBoundsTest()
    {
        Hex[,] grid = new Hex[5, 5];
        grid = _initializeMap(grid);

        Assert.IsTrue(MapUtilityMethods.IsOutOfBounds(grid, grid.GetLength(0), 0));
        Assert.IsTrue(MapUtilityMethods.IsOutOfBounds(grid, 0, grid.GetLength(1)));
        Assert.IsTrue(MapUtilityMethods.IsOutOfBounds(grid, 6, 0));
        Assert.IsFalse(MapUtilityMethods.IsOutOfBounds(grid, grid.GetLength(0) - 1, 1));
        Assert.IsFalse(MapUtilityMethods.IsOutOfBounds(grid, 2, grid.GetLength(0) - 1));
    }

    [Test]
    public void IsWallTest()
    {
        Hex[,] grid = new Hex[5, 5];
        grid = _initializeMap(grid);

        Assert.IsTrue(MapUtilityMethods.IsWall(grid, grid.GetLength(0), 0));
        Assert.IsTrue(MapUtilityMethods.IsWall(grid, grid.GetLength(0) - 1, 0));
        Assert.IsTrue(MapUtilityMethods.IsWall(grid, 10, 10));
        Assert.IsTrue(MapUtilityMethods.IsWall(grid, 2, 4));
        Assert.IsFalse(MapUtilityMethods.IsWall(grid, 1, 1));
        Assert.IsFalse(MapUtilityMethods.IsWall(grid, 2, 3));
    }
}
