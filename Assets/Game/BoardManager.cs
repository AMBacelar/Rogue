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

    public void Initialise()
    {
        if (instance == null) instance = this;
        else Debug.LogError("More than one BoardManager");
    }

    public void Awake()
    {
        Initialise();
        HexGrid = generator.GenerateMap(width, height);
        LoadMap(HexGrid);
    }

    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(xOffset * (x + y / 2f), 0, y * zOffset);
    }

    public void LoadMap(int[,] mapIn)
    {
        int x = mapIn.GetLength(0);
        int y = mapIn.GetLength(1);

        for (int column = 0, row = 0; row < y; row++)
        {
            for (column = 0; column < x; column++)
            {
                TileType tt = tileTypes[HexGrid[column, row]];
                GameObject hexagon = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(xOffset * (column + row / 2f), 0, row * zOffset), Quaternion.identity, this.transform);

                hexagon.name = "Hex_" + column + "_" + row;
                hexagon.isStatic = true;
            }
        }

        StaticBatchingUtility.Combine(this.gameObject);
    }

    public void LoadMap()
    {
        LoadMap(HexGrid);
    }

}