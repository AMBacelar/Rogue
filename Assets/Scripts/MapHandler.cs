using UnityEngine;
using System.Collections.Generic;

public class MapHandler
{


    //Size of the map in terms of hex tiles
    public int width;
    public int height;
    public int PercentAreWalls;

    GameObject selectedUnit;

    public float xOffset = 0.939f;
    public float zOffset = 0.814f;

    public int[,] HexGrid;
    Hex[] hex;

    int[,] tiles;
    Node[,] graph;

    public void MakeCaverns()
    {
        for (int column = 0, row = 0; row <= height - 1; row++)
        {
            for (column = 0; column <= width - 1; column++)
            {
                HexGrid[column, row] = PlaceWallLogic(column, row);
            }
        }
        //FLOOD CAVERN HERE
    }

    public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
    {

        Hex tt = hex[tiles[targetX, targetY]];

        if (UnitCanEnterTile(targetX, targetY) == false)
            return Mathf.Infinity;

        float cost = tt.movementCost;

        return cost;

    }

    public void GenerateNodeGraph()
    {
        graph = new Node[width, height];
        for (int column = 0, row = 0; row <= height - 1; row++)
        {
            for (column = 0; column <= width - 1; column++)
            {
                graph[column, row] = new Node();
                graph[column, row].x = column;
                graph[column, row].y = row;
            }
        }


        for (int column = 0, row = 0; row <= height - 1; row++)
        {
            for (column = 0; column <= width - 1; column++)
            {
                //LEFT
                if (column > 0)
                {
                    graph[column, row].neighbours.Add(graph[column - 1, row]);
                    //ODD ROW LEFT
                    if (row % 2 == 1)
                    {
                        //TOP LEFT TILE
                        if (row < height - 1)
                            graph[column, row].neighbours.Add(graph[column, row + 1]);
                        //BOTTOM LEFT TILE
                        if (row > 0)
                            graph[column, row].neighbours.Add(graph[column, row - 1]);
                    }
                    else //EVEN ROW LEFT
                    {
                        //TOP LEFT TILE
                        if (row < height - 1)
                            graph[column, row].neighbours.Add(graph[column - 1, row + 1]);
                        //BOTTOM LEFT TILE
                        if (row > 0)
                            graph[column, row].neighbours.Add(graph[column - 1, row - 1]);
                    }
                }
                //RIGHT
                if (column < width - 1)
                {
                    graph[column, row].neighbours.Add(graph[column + 1, row]);
                    //ODD ROW RIGHT
                    if (row % 2 == 1)
                    {
                        //TOP RIGHT TILE
                        if (row < height - 1)
                            graph[column, row].neighbours.Add(graph[column + 1, row + 1]);
                        //BOTTOM RIGHT TILE
                        if (row > 0)
                            graph[column, row].neighbours.Add(graph[column + 1, row - 1]);
                    }
                    else //EVEN ROW RIGHT
                    {
                        //TOP RIGHT TILE
                        if (row < height - 1)
                            graph[column, row].neighbours.Add(graph[column, row + 1]);
                        //BOTTOM RIGHT TILE
                        if (row > 0)
                            graph[column, row].neighbours.Add(graph[column, row - 1]);
                    }
                }
            }
        }
    }

    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        float xPos = x * xOffset;
        if (y % 2 == 1)
        {
            xPos += xOffset / 2f;
        }
        return new Vector3(xPos, y * zOffset, 0);
    }

    public bool UnitCanEnterTile(int x, int y)
    {

        // We could test the unit's walk/hover/fly type against various
        // terrain flags here to see if they are allowed to enter the tile.

        return hex[tiles[x, y]].isWalkable;
    }

    public void GeneratePathTo(int x, int y)
    {
        // Clear current path

        selectedUnit.GetComponent<Unit>().CurrentPath = null;

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        List<Node> unvisited = new List<Node>();

        Node source = graph[
            selectedUnit.GetComponent<Unit>().tileX,
            selectedUnit.GetComponent<Unit>().tileY
            ];
        Node target = graph[
            x,
            y
            ];

        dist[source] = 0;
        prev[source] = null;

        foreach (Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }

            unvisited.Add(v);
        }

        while (unvisited.Count > 0)
        {
            Node u = null;

            foreach (Node possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            if (u == target)
            {
                break;
            }

            unvisited.Remove(u);

            foreach (Node v in u.neighbours)
            {
                float alt = dist[u] + u.DistanceTo(v);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }
        if (prev[target] == null)
        {
            return;
        }
        List<Node> currentPath = new List<Node>();

        Node curr = target;

        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }

        currentPath.Reverse();

        selectedUnit.GetComponent<Unit>().CurrentPath = currentPath;
    }

    public int PlaceWallLogic(int x, int y)
    {
        int NumberOfWalls = GetAdjacentWalls(x, y, 1, 1);


        if (HexGrid[x, y] == 1)
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

    public int GetAdjacentWalls(int x, int y, int scopeX, int scopeY)
    {
        int startX = x - scopeX;
        int startY = y - scopeY;
        int endX = x + scopeX;
        int endY = y + scopeY;

        int iX = startX;
        int iY = startY;

        int WallCounter = 0;

        for (iY = startY; iY <= endY; iY++)
        {
            for (iX = startX; iX <= endX; iX++)
            {
                if (!(iX == x && iY == y))
                {
                    if (IsWall(iX, iY))
                    {
                        WallCounter += 1;
                    }
                }
            }
        }
        return WallCounter;
    }

    public bool IsWall(int x, int y)
    {

        // Consider out-of-bound a wall
        if (IsOutOfBounds(x, y))
        {
            return true;
        }

        if (HexGrid[x, y] == 1)
        {
            return true;
        }

        if (HexGrid[x, y] == 0)
        {
            return false;
        }
        return false;
    }

    public bool IsOutOfBounds(int x, int y)
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

    public void LoadMap(GameObject wallTile, GameObject floorTile, GameObject fillTile, GameObject parent)
    {
        List<GameObject> TileType = new List<GameObject>();
        TileType.Add(floorTile);
        TileType.Add(wallTile);
        TileType.Add(fillTile);

        for (int column = 0, row = 0; row < height; row++)
        {
            for (column = 0; column < width; column++)
            {
                float xPos = column * xOffset;
                if (row % 2 == 1)
                {
                    xPos += xOffset / 2f;
                }

                GameObject hexagon = (GameObject)GameObject.Instantiate(TileType[HexGrid[column, row]], new Vector3(xPos, 0, row * zOffset), Quaternion.identity);

                string tileName = "";

                if (HexGrid[column, row] == 0)
                {
                    tileName = "floorTile_";
                }
                else if (HexGrid[column, row] == 1)
                {
                    tileName = "wallTile_";
                }
                else if (HexGrid[column, row] == 2)
                {
                    tileName = "fillTile_";
                }

                hexagon.name = tileName + "Hex_" + column + "_" + row;

                hexagon.transform.SetParent(parent.transform);

                hexagon.isStatic = true;
            }
        }
    }

    public MapHandler(int MapWidth, int MapHeight, int[,] map, int PercentAreWalls = 47)
    {
        this.width = MapWidth;
        this.height = MapHeight;
        this.PercentAreWalls = PercentAreWalls;
        this.HexGrid = map;
    }

    public MapHandler()
    {
        width = 120;
        height = 60;
        PercentAreWalls = 47;

        HexGrid = new int[width, height];

        RandomFillMap();
    }

    public void BlankMap()
    {
        for (int column = 0, row = 0; row < height; row++)
        {
            for (column = 0; column < width; column++)
            {
                HexGrid[column, row] = 0;
            }
        }
    }

    public void RandomFillMap()
    {

        // New, empty map
        HexGrid = new int[width, height];

        int mapMiddle = 0; // Temp variable
        for (int column = 0, row = 0; row < height; row++)
        {
            for (column = 0; column < width; column++)
            {
                // If coordinants lie on the the edge of the map (creates a border)
                if (column == 0)
                {
                    HexGrid[column, row] = 1;
                }
                else if (row == 0)
                {
                    HexGrid[column, row] = 1;
                }
                else if (column == width - 1)
                {
                    HexGrid[column, row] = 1;
                }
                else if (row == height - 1)
                {
                    HexGrid[column, row] = 1;
                }
                // Else, fill with a wall a random percent of the time
                else
                {
                    mapMiddle = (height / 2);

                    if (row == mapMiddle)
                    {
                        HexGrid[column, row] = 0;
                    }
                    else
                    {
                        HexGrid[column, row] = RandomPercent(PercentAreWalls);
                    }
                }
            }
        }
    }

    int RandomPercent(int percent)
    {
        if (percent >= Random.Range(0, 100))
        {
            return 1;
        }
        return 0;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
