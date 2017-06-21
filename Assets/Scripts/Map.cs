using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
    public MapHandler ActiveMap;
    public TileType[] tileTypes;

    public int width, height, percentAreWalls;


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
        MapHandler ActiveMap = gameObject.AddComponent<MapHandler>();
        
        ActiveMap.tileTypes = tileTypes;

        ActiveMap.MakeCaverns();

        ActiveMap.GeneratePathfindingGraph();

        GameObject selectedUnit = GameObject.FindGameObjectWithTag("Player");

        selectedUnit.GetComponent<Unit>().tileY = (int)(selectedUnit.transform.position.z / ActiveMap.zOffset);

        float xPos = selectedUnit.transform.position.x / ActiveMap.xOffset;
        if (selectedUnit.GetComponent<Unit>().tileY % 2 != 1)
        {
            xPos += ActiveMap.xOffset / 2f;
        }

        selectedUnit.GetComponent<Unit>().tileX = (int)(xPos);

        selectedUnit.GetComponent<Unit>().map = ActiveMap;

        FloodMap(ActiveMap.HexGrid, ActiveMap.width / 2, ActiveMap.height / 2, 2, 1); // Fill Val is set to 2 because that is the red tile, visual for debugging, boundary value is 1 because that is the value for a wall tile(impassable terrain)

        if (CheckFlooding(ActiveMap.HexGrid, ActiveMap.height - 1, ActiveMap.width - 1) == true)
        {
            Debug.Log("success!");
            DrainMap(ActiveMap.HexGrid, ActiveMap.width - 1, ActiveMap.height - 1);
            ActiveMap.LoadMap(this.gameObject);
        }
        else
        {
            Debug.Log("aww Shucks");
            LoadMap();
        }
    }
    
    void Start()
    {
        LoadMap();
    }
}
