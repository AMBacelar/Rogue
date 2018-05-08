using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public static BoardManager instance = null;
    public int width;
    public int height;
    public int depth = 1;
    public int monsterDensity = 30;

    public float xOffset = 0.939f;
    public float zOffset = 0.814f;

    public Hex[,] HexGrid;
    public Node[,] graph;
    private Dictionary<IntVector2, BoardPosition> dynamicPositions = new Dictionary<IntVector2, BoardPosition>();

    public TileType[] tileTypes;

    MapGenerator generator = new MapGenerator();
    FeatureGenerator featureGenerator = new FeatureGenerator();

    public void Initialise()
    {
        if (instance == null) instance = this;
        else Debug.LogError("More than one BoardManager");
    }

    public void Awake()
    {
        Initialise();
        HexGrid = generator.GenerateMap(width, height);
        graph = Astar.GenerateGraph(HexGrid);
        LoadMap(HexGrid);
        Populate();
    }

    public void Populate()
    {
        foreach (var hex in HexGrid)
        {
            if (IsOccupied(new IntVector2(hex.Q, hex.R)) == false && IsPassable(hex.Q, hex.R))
                featureGenerator.Populate(new IntVector2(hex.Q, hex.R), monsterDensity, depth);
        }
    }

    public void UnregisterDynamicBoardPosition(BoardPosition toUnregister)
    {
        if (!dynamicPositions.ContainsKey(toUnregister.Position) || dynamicPositions[toUnregister.Position] != toUnregister)
        {
            Debug.LogError(toUnregister.name + "is not registered at it's board position.");
            return;
        }
        dynamicPositions[toUnregister.Position] = null;
    }

    public void RegisterDynamicBoardPosition(BoardPosition toRegister)
    {
        if (MapUtilityMethods.IsOutOfBounds(HexGrid, toRegister.X, toRegister.Y))
        {
            Debug.LogError("Attempt to RegisterActor out of board boaunds");
            return;
        }

        if (IsOccupied(toRegister.Position))
        {
            Debug.LogError("Attempt to RegisterActor occupied board position " + GetOccupied(toRegister.Position).name);
            return;
        }
        dynamicPositions[toRegister.Position] = toRegister;
    }

    public bool IsOccupied(Hex position){
        return IsOccupied(new IntVector2(position.Q,position.R));
    }

    public bool IsOccupied(Node node){
        return IsOccupied(new IntVector2(node.x,node.y));
    }

    public bool IsOccupied(IntVector2 position)
    {
        return GetOccupied(position) != null;
    }

    public BoardPosition GetOccupied(IntVector2 position)
    {
        if (!dynamicPositions.ContainsKey(position))
        {
            return null;
        }
        return dynamicPositions[position];
    }

    public bool IsWithinBounds(int x, int y)
    {
        return !MapUtilityMethods.IsOutOfBounds(HexGrid, x, y);
    }

    public bool IsPassable(int x, int y)
    {
        return HexGrid[x, y].isWalkable && IsWithinBounds(x, y) && HexGrid[x, y].tileType == 0;
    }

    public bool IsPassable(IntVector2 position)
    {
        return IsPassable(position.X, position.Y);
    }

    public Hex GetHex(int x, int y)
    {
        return HexGrid[x, y];
    }

    public Hex GetHex(IntVector2 pos)
    {
        return GetHex(pos.X, pos.Y);
    }

    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(xOffset * (x + y / 2f), 0, y * zOffset);
    }

    public int Heuristics(Node a, Node b)
    {
        return Heuristics(new Hex(a.x, a.y), new Hex(b.x, b.y));
    }

    public int Heuristics(IntVector2 a, IntVector2 b)
    {
        return Heuristics(new Hex(a.X, a.Y), new Hex(b.X, b.Y));
    }

    public int Heuristics(BoardPosition a, BoardPosition b)
    {
        return Heuristics(new Hex(a.X, a.Y), new Hex(b.X, b.Y));
    }

    public int Heuristics(Hex a, Hex b)
    {
        return MapUtilityMethods.Heuristics(a, b);
    }

    public void LoadMap(Hex[,] mapIn)
    {
        int x = mapIn.GetLength(0);
        int y = mapIn.GetLength(1);

        for (int column = 0, row = 0; row < y; row++)
        {
            for (column = 0; column < x; column++)
            {
                TileType tt = tileTypes[HexGrid[column, row].tileType];
                GameObject hexagon = Instantiate(tt.tileVisualPrefab, TileCoordToWorldCoord(column, row), Quaternion.identity, transform);
                hexagon.name = "Hex_" + column + "_" + row;
                hexagon.isStatic = true;
            }
        }
        StaticBatchingUtility.Combine(gameObject);
    }

    public void LoadMap()
    {
        LoadMap(HexGrid);
    }
}