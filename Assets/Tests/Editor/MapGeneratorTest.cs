using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MapGeneratorTest
{
    MapGenerator generator = new MapGenerator();

    [Test]
    public void RandomPercentTest()
    {
        int percent = 47;
        Assert.AreEqual(generator.RandomPercent(percent, 02), 1);
        Assert.AreEqual(generator.RandomPercent(percent, 47), 1);
        Assert.AreEqual(generator.RandomPercent(percent, 48), 0);
        Assert.AreEqual(generator.RandomPercent(percent, 80), 0);
    }

    [Test]
    public void EvenGetAdjacentWallsTest()
    {
        int[,] testMap1 = {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 }
        };
        /*
                    . . . . .
                     . . . . .
                    . . @ . .
                     . . . . .
                    . . . . .
        */

        Assert.AreEqual(0, generator.GetAdjacentWalls(testMap1, 2, 2));

        int[,] testMap2 = {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 }
        };
        /*
                    . . . . .
                     . . . . .
                    . . @ . .
                     . . p . .
                    . . . . .
        */

        Assert.AreEqual(1, generator.GetAdjacentWalls(testMap2, 2, 2));

        int[,] testMap3 = {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 1, 0, 0, 0 },
            { 0, 0, 0, 0, 0 }
        };
        /*
                    . . . . .
                     . . p . .
                    . . @ . .
                     . . p . .
                    . . . . .
        */

        Assert.AreEqual(2, generator.GetAdjacentWalls(testMap3, 2, 2));

        int[,] testMap4 = {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0 }
        };
        /*
                    . . . . .
                     . . p . .
                    . . @ p .
                     . . p . .
                    . . . . .
        */

        Assert.AreEqual(3, generator.GetAdjacentWalls(testMap4, 2, 2));

        int[,] testMap5 = {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 1, 1, 1, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0 }
        };
        /*
                    . . . . .
                     . p p . .
                    . . @ p .
                     . . p . .
                    . . . . .
        */

        Assert.AreEqual(4, generator.GetAdjacentWalls(testMap5, 2, 2));

        int[,] testMap6 = {
            { 0, 0, 0, 0, 0 },
            { 0, 1, 0, 0, 0 },
            { 0, 1, 1, 1, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0 }
        };
        /*
                    . . . . .
                     . p p . .
                    . . @ p .
                     . p p . .
                    . . . . .
        */

        Assert.AreEqual(5, generator.GetAdjacentWalls(testMap6, 2, 2));

        int[,] testMap7 = {
            { 0, 0, 0, 0, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0 }
        };
        /*
                    . . . . .
                     . p p . .
                    . p @ p .
                     . p p . .
                    . . . . .
        */

        Assert.AreEqual(6, generator.GetAdjacentWalls(testMap7, 2, 2));

        int[,] testMap8 = {
            { 0, 1, 1, 1, 0 },
            { 1, 0, 0, 1, 0 },
            { 1, 0, 0, 0, 1 },
            { 1, 0, 0, 1, 0 },
            { 0, 1, 1, 1, 0 }
        };
        /*
                    . p p p .
                     p . . p .
                    p . @ . p
                     p . . p .
                    . p p p .
        */

        Assert.AreEqual(0, generator.GetAdjacentWalls(testMap8, 2, 2));
    }

    [Test]
    public void OddGetAdjacentWallsTest()
    {
        int[,] testMap1 = {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 }
        };
        /*
                    . . . . . .
                     . . . . . .
                    . . . @ . .
                     . . . . . .
                    . . . . . .
                     . . . . . .
        */

        Assert.AreEqual(0, generator.GetAdjacentWalls(testMap1, 3, 3));

        int[,] testMap2 = {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0 }
        };
        /*
                    . . . . . .
                     . . . . . .
                    . . . @ p .
                     . . . . . .
                    . . . . . .
                     . . . . . .
        */

        Assert.AreEqual(1, generator.GetAdjacentWalls(testMap2, 3, 3));

        int[,] testMap3 = {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0 }
        };
        /*
                    . . . . . .
                     . . . . . .
                    . . p @ p .
                     . . . . . .
                    . . . . . .
                     . . . . . .
        */

        Assert.AreEqual(2, generator.GetAdjacentWalls(testMap3, 3, 3));

        int[,] testMap4 = {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 0 },
            { 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0 }
        };
        /*
                    . . . . . .
                     . . p . . .
                    . . p @ p .
                     . . . . . .
                    . . . . . .
                     . . . . . .
        */

        Assert.AreEqual(3, generator.GetAdjacentWalls(testMap4, 3, 3));

        int[,] testMap5 = {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 0 },
            { 0, 0, 1, 1, 0, 0 },
            { 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0 }
        };
        /*
                    . . . . . .
                     . . p . . .
                    . . p @ p .
                     . . . p . .
                    . . . . . .
                     . . . . . .
        */

        Assert.AreEqual(4, generator.GetAdjacentWalls(testMap5, 3, 3));

        int[,] testMap6 = {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 0 },
            { 0, 0, 1, 1, 0, 0 },
            { 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0 }
        };
        /*
                    . . . . . .
                     . . p . . .
                    . . p @ p .
                     . . p p . .
                    . . . . . .
                     . . . . . .
        */

        Assert.AreEqual(5, generator.GetAdjacentWalls(testMap6, 3, 3));

        int[,] testMap7 = {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 0 },
            { 0, 0, 1, 1, 1, 0 },
            { 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0 }
        };
        /*
                    . . . . . .
                     . . p p . .
                    . . p @ p .
                     . . p p . .
                    . . . . . .
                     . . . . . .
        */

        Assert.AreEqual(6, generator.GetAdjacentWalls(testMap7, 3, 3));

        int[,] testMap8 = {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 0 },
            { 0, 1, 0, 0, 0, 1 },
            { 0, 1, 0, 1, 0, 1 },
            { 0, 1, 1, 0, 1, 1 },
            { 0, 0, 0, 1, 0, 0 }
        };
        /*
                    . . p p p .
                     . p . . p .
                    . p . @ . p
                     . p . . p .
                    . . p p p .
                     . . . . . .
        */

        Assert.AreEqual(0, generator.GetAdjacentWalls(testMap8, 3, 3));

    }
}