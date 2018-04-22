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
}