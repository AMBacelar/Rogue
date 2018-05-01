using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{

    public Hex[,] GenerateMap(int width, int height)
    {
        Hex[,] tileGrid = new Hex[width, height];
        tileGrid = RandomFillMap(tileGrid, 53);
        tileGrid = MakeCaverns(tileGrid);
        FloodMap(tileGrid, width / 2, height / 2, 0, 2, 1);
        tileGrid = DrainMap(tileGrid, 2, 0, 1);
        if (CheckFlooding(tileGrid, 0) == false)
        {
            Debug.Log("aww Shucks");
            GenerateMap(width, height);
        }
        else
        {
            Debug.Log("success!");
        }
        return tileGrid;
    }

    public Hex[,] RandomFillMap(Hex[,] mapIn, int percentIsFloor)
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
                // Else, fill with a wall a random percent of the time
                else
                {
                    int mapMiddle = (y / 2);

                    if (row == mapMiddle)
                    {
                        mapIn[column, row] = new Hex(true, 0, column, row);
                    }
                    else if (row == mapMiddle+1)
                    {
                        mapIn[column, row] = new Hex(true, 0, column, row);
                    }
                    else
                    {
                        bool isFloor = RandomPercent(percentIsFloor, Random.Range(0, 100));
                        mapIn[column, row] = new Hex(isFloor, isFloor ? 0 : 1, column, row);
                    }
                }

            }
        }
        return mapIn;
    }

    public bool RandomPercent(int percent, int random)
    {
        return percent >= random ? true : false;
    }

    public Hex[,] MakeCaverns(Hex[,] mapIn)
    {
        int width = mapIn.GetLength(0);
        int height = mapIn.GetLength(1);
        Hex[,] newMap = new Hex[width, height];
        for (int row = 0; row <= height - 1; row++)
        {
            for (int column = 0; column <= width - 1; column++)
            {
                newMap[column, row] = PlaceWallLogic(mapIn, column, row);
            }
        }
        return newMap;
    }

    public Hex PlaceWallLogic(Hex[,] mapIn, int x, int y)
    {
        int NumberOfWalls = GetAdjacentWalls(mapIn, x, y);

        if (!mapIn[x, y].isWalkable)
        {
            if (NumberOfWalls >= 3)
            {
                return new Hex(false, 1, x, y);
            }
            return new Hex(true, 0, x, y);
        }
        if (NumberOfWalls > 3)
        {
            return new Hex(false, 1, x, y);
        }
        return new Hex(true, 0, x, y);
    }

    public int GetAdjacentWalls(Hex[,] mapIn, int x, int y)
    {
        // receive a position on the board, return how many neighbours it has

        Hex[] neighbours = mapIn[x, y].GetNeighbours();

        int Untraversable = 0;

        foreach (Hex h in neighbours)
        {
            if (MapUtilityMethods.IsWall(mapIn, h.Q, h.R))
            {
                Untraversable++;
            }
        }
        return Untraversable;
    }

    public void FloodMap(Hex[,] mapIn, int x, int y, int searchVal, int fillVal, int boundaryVal)
    {
        if (mapIn[x, y].tileType == searchVal)
        {
            if (mapIn[x, y].tileType != boundaryVal && mapIn[x, y].tileType != fillVal)
            {
                mapIn[x, y].tileType = fillVal;

                Hex[] neighbours = mapIn[x, y].GetNeighbours();

                foreach (var h in neighbours)
                {
                    FloodMap(mapIn, h.Q, h.R, searchVal, fillVal, boundaryVal);
                }
            }
        }
    }

    public bool CheckFlooding(Hex[,] mapIn, int searchValue)
    {
        int searchCount = 0;
        int totalTiles = 0;

        foreach (Hex h in mapIn)
        {
            totalTiles++;
            if (h.tileType == searchValue)
            {
                searchCount++;
            }
        }
        if (searchCount == 0) return false;
        int NeedsToBeMoreThan = (totalTiles / 10) * 3;
        return searchCount > NeedsToBeMoreThan;
    }

    public Hex[,] DrainMap(Hex[,] mapIn, int convertThisValue, int intoThisValue, int everythingElse)
    {
        foreach (Hex t in mapIn)
        {
            if (t.tileType == intoThisValue)
            {
                t.tileType = everythingElse;
            }
        }
        foreach (Hex t in mapIn)
        {
            if (t.tileType == convertThisValue)
            {
                t.tileType = intoThisValue;
            }
        }
        return mapIn;
    }
}