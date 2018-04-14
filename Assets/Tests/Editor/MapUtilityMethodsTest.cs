using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MapUtilityMethodsTest
{

    [Test]
    public void IsOutOfBoundsTest()
    {
        int[,] testMap = new int[5, 5];
        Assert.IsTrue(MapUtilityMethods.IsOutOfBounds(testMap, testMap.GetLength(0), 0));
        Assert.IsTrue(MapUtilityMethods.IsOutOfBounds(testMap, 0, testMap.GetLength(1)));
        Assert.IsTrue(MapUtilityMethods.IsOutOfBounds(testMap, 6, 0));
        Assert.IsFalse(MapUtilityMethods.IsOutOfBounds(testMap, testMap.GetLength(0) - 1, 1));
        Assert.IsFalse(MapUtilityMethods.IsOutOfBounds(testMap, 2, testMap.GetLength(0) - 1));
    }

    [Test]
    public void IsWallTest()
    {
        int[,] testMap = new int[,] {
            { 1, 1, 1, 1, 1 },
            { 1, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0 },
            { 1, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1 }
        };

        Assert.IsTrue(MapUtilityMethods.IsWall(testMap, testMap.GetLength(0), 0));
        Assert.IsTrue(MapUtilityMethods.IsWall(testMap, testMap.GetLength(0) - 1, 0));
        Assert.IsTrue(MapUtilityMethods.IsWall(testMap, 10, 10));
        Assert.IsFalse(MapUtilityMethods.IsWall(testMap, 1, 1));
        Assert.IsFalse(MapUtilityMethods.IsWall(testMap, 2, 4));
    }
}
