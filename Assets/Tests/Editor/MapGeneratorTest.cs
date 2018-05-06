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
        Assert.AreEqual(true, generator.RandomPercent(percent, 02));
        Assert.AreEqual(true, generator.RandomPercent(percent, 47));
        Assert.AreEqual(false, generator.RandomPercent(percent, 48));
        Assert.AreEqual(false, generator.RandomPercent(percent, 80));
    }

    [Test]
    public void PlaceWallLogicTest()
    {

    }

}