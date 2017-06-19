using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour
{

    public GameObject floorHexPrefab;
    public GameObject wallHexPrefab;
    public GameObject fillHexPrefab;     

    public void FloodMap(int[,] mapIn, int x, int y, int fillVal, int boundaryVal)
    {
        int curVal;

        if (mapIn[x, y] == 0)
        {
            curVal = mapIn[x, y];
            if (curVal != boundaryVal && curVal != fillVal)
            {
                mapIn[x, y] = fillVal;
                FloodMap(mapIn, x + 1, y, fillVal, boundaryVal);
                FloodMap(mapIn, x - 1, y, fillVal, boundaryVal);
                FloodMap(mapIn, x, y + 1, fillVal, boundaryVal);
                FloodMap(mapIn, x, y - 1, fillVal, boundaryVal);
            }
        }
    }

    public bool CheckFlooding(int[,] FloodedMap, int x, int y)
    {
        Dictionary<int, int> TileCount = new Dictionary<int, int>();

        foreach (int value in FloodedMap)
        {
            if (TileCount.ContainsKey(value))
                TileCount[value]++;
            else
                TileCount.Add(value, 1);
        }
        if (TileCount.ContainsKey(2))
        {
            int NeedsToBeMoreThan = ((TileCount[1] + TileCount[0]) / 5) * 3;
            return (TileCount[2] > NeedsToBeMoreThan) ? true : false;
        }
        else
        {
            return false;
        }
    }

    public void DrainMap(int[,] MapIn, int x, int y)
    {
        for (int u = 0; u < x; u++)
        {
            for (int v = 0; v < y; v++)
            {
                if (MapIn[u, v] != 2)
                {
                    MapIn[u, v] = 1;
                }
            }
        }
        for (int u = 0; u < x; u++)
        {
            for (int v = 0; v < y; v++)
            {
                if (MapIn[u, v] == 2)
                {
                    MapIn[u, v] = 0;
                }
            }
        }
    }

    public void LoadMap()
    {
        MapHandler Map = new MapHandler();

        Map.MakeCaverns();
        FloodMap(Map.HexGrid, Map.width / 2, Map.height / 2, 2, 1); // Fill Val is set to 2 because that is the red tile, visual for debugging, boundary value is 1 because that is the value for a wall tile(impassable terrain)

        if (CheckFlooding(Map.HexGrid, Map.height - 1, Map.width - 1) == true)
        {
            Debug.Log("success!");
            DrainMap(Map.HexGrid, Map.width - 1, Map.height - 1);
            Map.LoadMap(wallHexPrefab, floorHexPrefab, fillHexPrefab, this.gameObject);
        }
        else
        {
            Debug.Log("aww Shucks");
            LoadMap();
        }
    }
    // Use this for initialization
    void Start()
    {
        LoadMap();        
    }
}
