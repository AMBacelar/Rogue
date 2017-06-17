using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour
{

    public GameObject floorHexPrefab;
    public GameObject wallHexPrefab;
    public GameObject fillHexPrefab;



    public class MapHandler
    {
        //Size of the map in terms of hex tiles
        public int width;
        public int height;
        public int PercentAreWalls;

        public float xOffset = 0.939f;
        public float zOffset = 0.814f;

        public int[,] HexGrid;
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

        public class Node
        {
            public List<Node> neighbours;

            public Node()
            {
                neighbours = new List<Node>();
            }
        }

        public void NodeGraph()
        {
            graph = new Node[width, height];
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

                    GameObject hexagon = (GameObject)Instantiate(TileType[HexGrid[column, row]], new Vector3(xPos, 0, row * zOffset), Quaternion.identity);

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
