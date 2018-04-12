using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

  public static BoardManager instance = null;
  public int width;
  public int height;

  public float xOffset = 0.939f;
  public float zOffset = 0.814f;

  public int[,] HexGrid;
  Node[,] graph;

  public TileType[] tileTypes;

  MapGenerator generator = new MapGenerator();

  void Awake()
  {
    if (instance == null) instance = this;
    else Debug.LogError("More than one BoardManager");

    HexGrid = generator.GenerateMap(width, height);
    LoadMap(HexGrid);
  }

  public Vector3 TileCoordToWorldCoord(int x, int y)
  {
    float xPos = x * xOffset;
    if (y % 2 == 1)
    {
      xPos += xOffset / 2f;
    }
    return new Vector3(xPos, 0, y * zOffset);
  }

  public bool IsOutOfBounds(int x, int y)
  {
    return IsOutOfBounds(HexGrid, x, y);
  }

  public bool IsOutOfBounds(int[,] mapIn, int x, int y)
  {
    if (x < 0 || y < 0)
    {
      return true;
    }
    else if (x > width - 1 || y > height - 1)
    {
      return true;
    }
    return false;
  }

  public bool IsWall(int x, int y)
  {

    return IsWall(HexGrid, x, y);
  }

  public bool IsWall(int[,] mapIn, int x, int y)
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

  public void LoadMap(int[,] mapIn)
  {
    int x = mapIn.GetLength(0);
    int y = mapIn.GetLength(1);

    for (int column = 0, row = 0; row < y; row++)
    {
      for (column = 0; column < x; column++)
      {
        float xPos = column * xOffset;
        if (row % 2 == 1)
        {
          xPos += xOffset / 2f;
        }

        TileType tt = tileTypes[HexGrid[column, row]];
        GameObject hexagon = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(xPos, 0, row * zOffset), Quaternion.identity);

        hexagon.name = "Hex_" + column + "_" + row;

        //hexagon.transform.SetParent(parent.transform);
        hexagon.isStatic = true;
      }
    }
  }

  public void LoadMap()
  {
    LoadMap(HexGrid);
  }

}