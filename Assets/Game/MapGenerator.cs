using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{

  public int[,] GenerateMap(int width, int height)
  {
    int[,] tileGrid = new int[width, height];
    tileGrid = RandomFillMap(tileGrid, 47);
    tileGrid = MakeCaverns(tileGrid);
    FloodMap(tileGrid, width / 2, height / 2, 2, 1);
    if (CheckFlooding(tileGrid, 2, 0, 1) == true)
    {
      Debug.Log("success!");
      tileGrid = DrainMap(tileGrid, 2, 1, 0);
    }
    else
    {
      Debug.Log("aww Shucks");
      GenerateMap(width, height);
    }
    return tileGrid;
  }

  public int[,] RandomFillMap(int[,] mapIn, int percentAreWalls)
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
          mapIn[column, row] = 1;
        }
        else if (row == 0)
        {
          mapIn[column, row] = 1;
        }
        else if (column == x - 1)
        {
          mapIn[column, row] = 1;
        }
        else if (row == y - 1)
        {
          mapIn[column, row] = 1;
        }
        // Else, fill with a wall a random percent of the time
        else
        {
          int mapMiddle = (y / 2);

          if (row == mapMiddle)
          {
            mapIn[column, row] = 0;
          }
          else
          {
            mapIn[column, row] = RandomPercent(percentAreWalls, Random.Range(0, 100));
          }
        }

      }
    }
    return mapIn;
  }

  int RandomPercent(int percent, int random)
  {
    return percent >= random ? 1 : 0;
  }

  public int[,] MakeCaverns(int[,] mapIn)
  {
    int width = mapIn.GetLength(0);
    int height = mapIn.GetLength(1);
    for (int row = 0; row <= height - 1; row++)
    {
      for (int column = 0; column <= width - 1; column++)
      {
        mapIn[column, row] = PlaceWallLogic(mapIn, column, row);
      }
    }
    return mapIn;
  }

  public int PlaceWallLogic(int[,] mapIn, int x, int y)
  {
    int NumberOfWalls = GetAdjacentWalls(mapIn, x, y);

    if (mapIn[x, y] == 1)
    {
      if (NumberOfWalls >= 4)
      {
        return 1;
      }
      if (NumberOfWalls < 2)
      {
        return 0;
      }
    }
    else
    {
      if (NumberOfWalls >= 5)
      {
        return 1;
      }
    }
    return 0;
  }

  public int GetAdjacentWalls(int[,] mapIn, int x, int y)
  {
    // receive a position on the board, return how many neighbours it has

    int startX = x - 1;
    int startY = y - 1;
    int endX = x + 1;
    int endY = y + 1;

    int iX = startX;
    int iY = startY;

    int WallCounter = 0;

    for (iY = startY; iY <= endY; iY++)
    {
      for (iX = startX; iX <= endX; iX++)
      {
        if (!(iX == x && iY == y))
        {
          if (BoardManager.instance.IsWall(mapIn, iX, iY))
          {
            WallCounter += 1;
          }
        }
      }
    }
    return WallCounter;
  }

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

  public bool CheckFlooding(int[,] mapIn, int floodValue, int value1, int value2 = 0)
  {
    Dictionary<int, int> TileCount = new Dictionary<int, int>();

    TileCount.Add(floodValue, 0);
    TileCount.Add(value1, 0);
    TileCount.Add(value2, 0);

    foreach (int value in mapIn)
    {
      if (TileCount.ContainsKey(value))
        TileCount[value]++;
      else
        TileCount.Add(value, 1);
    }
    if (TileCount.ContainsKey(floodValue))
    {
      int NeedsToBeMoreThan = ((TileCount[value1] + TileCount[value2]) / 5) * 3;
      return TileCount[floodValue] > NeedsToBeMoreThan;
    }
    else
    {
      return false;
    }
  }

  public int[,] DrainMap(int[,] mapIn, int convertThisValue, int intoThisValue, int everythingElse)
  {
    for (int u = 0; u < mapIn.GetLength(0); u++)
    {
      for (int v = 0; v < mapIn.GetLength(1); v++)
      {
        if (mapIn[u, v] != convertThisValue)
        {
          mapIn[u, v] = intoThisValue;
        }
      }
    }
    for (int u = 0; u < mapIn.GetLength(0); u++)
    {
      for (int v = 0; v < mapIn.GetLength(1); v++)
      {
        if (mapIn[u, v] == convertThisValue)
        {
          mapIn[u, v] = everythingElse;
        }
      }
    }
    return mapIn;
  }
}