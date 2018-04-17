using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapHandler : MonoBehaviour
{
    //Size of the map in terms of hex tiles
    public int width;
    public int height;
    public int PercentAreWalls;

    public GameObject selectedUnit;

    public float xOffset = 0.939f;
    public float zOffset = 0.814f;

    public int[,] HexGrid;
    public int[,] ProximityGrid;

    public TileType[] tileTypes;

    Node[,] graph;

    #region Pathfinding
    public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
    {

        TileType tt = tileTypes[HexGrid[targetX, targetY]];

        if (UnitCanEnterTile(targetX, targetY) == false)
            return Mathf.Infinity;

        float cost = tt.movementCost;

        return cost;

    }

    public void GeneratePathfindingGraph()
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
        return new Vector3(xPos, 0, y * zOffset);
    }

    public bool UnitCanEnterTile(int x, int y)
    {
        // We could test the unit's walk/hover/fly type against various
        // terrain flags here to see if they are allowed to enter the tile.
        return tileTypes[HexGrid[x, y]].isWalkable;
    }

    public IEnumerator GeneratePathTo(int x, int y, GameObject player)
    {
        // Clear current path
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Unit>().CurrentPath = null;

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        List<Node> unvisited = new List<Node>();

        Node source = graph[
            player.GetComponent<Unit>().tileX,
            player.GetComponent<Unit>().tileY
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

            int batch = 0;
            foreach (Node v in u.neighbours)
            {
                batch++;
                if (batch == 500)
                {
                    yield return new WaitForSeconds(0);
                    batch = 0;
                }
                float alt = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }

            }
        }
        if (prev[target] == null)
        {
            yield break;
        }
        List<Node> currentPath = new List<Node>();

        Node curr = target;

        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }

        currentPath.Reverse();

        player.GetComponent<Unit>().CurrentPath = currentPath;

        yield break;
    }
    #endregion

    #region HexGrid
    public void MakeCaverns()
    {
        for (int column = 0, row = 0; row <= height - 1; row++)
        {
            for (column = 0; column <= width - 1; column++)
            {
                HexGrid[column, row] = PlaceWallLogic(column, row);
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

    public void isPit(int threshold, int replaceWith)
    {
        // search through all the wall tiles in the map, once it finds a wall tile, it is to check it's neighbours
        // the wall tile and it's neighbours are added to a queue, to be worked, you are to count all of the wall 
        // tiles that are in contact with each other, and if they are lower than a threshold then set them to be a
        // pit tile.

        bool[,] visited = new bool[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                visited[i, j] = false;
            }
        }

        LinkedList<Node> queue = new LinkedList<Node>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (IsWall(i, j) && visited[i, j] == false)
                {
                    queue.AddLast(graph[i, j]);

                    Node Start = queue.First();

                    List<Node> firstRep = Start.neighbours;

                    for (int a = 0; a < firstRep.Count; a++)
                    {
                        if (!visited[firstRep[a].x, firstRep[a].y] && IsWall(firstRep[a].x, firstRep[a].y))
                        {
                            visited[firstRep[a].x, firstRep[a].y] = true;
                            queue.AddLast(graph[firstRep[a].x, firstRep[a].y]);
                        }
                    }
                    queue.RemoveFirst();

                    int wallCount = 0;

                    List<Node> wallTileGroup = new List<Node>();

                    while (queue.Any())
                    {
                        Node S = queue.First();

                        List<Node> list = S.neighbours;

                        for (int a = 0; a < list.Count; a++)
                        {
                            if (!visited[list[a].x, list[a].y] && IsWall(list[a].x, list[a].y))
                            {
                                visited[list[a].x, list[a].y] = true;
                                wallTileGroup.Add(list[a]);
                                queue.AddLast(graph[list[a].x, list[a].y]);
                                wallCount++;
                            }
                        }
                        queue.RemoveFirst();
                    }
                    Debug.Log(wallCount);
                    if (wallCount < threshold)
                    {
                        // get a list of tile coordinates that need to be changed to red tiles
                        for (var n = 0; n < wallTileGroup.Count; n++)
                        {
                            HexGrid[wallTileGroup[n].x, wallTileGroup[n].y] = replaceWith;
                        }

                    }
                }
            }
        }

        List<Node> onlyIfItsAWall = new List<Node>();

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

    public void LoadMap(GameObject parent)
    {
        for (int column = 0, row = 0; row < height; row++)
        {
            for (column = 0; column < width; column++)
            {
                float xPos = column * xOffset;
                if (row % 2 == 1)
                {
                    xPos += xOffset / 2f;
                }

                TileType tt = tileTypes[HexGrid[column, row]];
                GameObject hexagon = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(xPos, 0, row * zOffset), Quaternion.identity);

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
                if (HexGrid[column, row] == 1)
                {
                    graph[column, row].isTraversable = false;
                }
                else
                {
                    graph[column, row].isTraversable = true;
                }
            }
        }
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
        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
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
                        HexGrid[column, row] = RandomPercent(PercentAreWalls, Random.Range(0, 100));
                    }
                }

            }
        }
    }

    int RandomPercent(int percent, int random)
    {
        return percent >= random ? 1 : 0;
    }
    #endregion

    #region ProximityGrid

    public void InitializeProximityGrid(int MaxValue, int StartPositionX, int StartPositionY)
    {
        ProximityGrid = new int[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                ProximityGrid[i, j] = MaxValue;
            }
        }
        ProximityGrid[StartPositionX, StartPositionY] = 0;
    }

    public void BreadthFirstTraversal(int StartPositionX, int StartPositionY)
    {
        bool[,] visited = new bool[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                visited[i, j] = false;
            }
        }

        LinkedList<Node> queue = new LinkedList<Node>();

        queue.AddLast(graph[StartPositionX, StartPositionY]);

        int value = 0;
        int layerProgress = 0;
        int LayerMultiplier = 1;

        Node Start = queue.First();
        ProximityGrid[Start.x, Start.y] = value;
        value++;

        List<Node> firstRep = Start.neighbours;

        for (int a = 0; a < firstRep.Count; a++)
        {
            if (!visited[firstRep[a].x, firstRep[a].y])
            {
                visited[firstRep[a].x, firstRep[a].y] = true;
                queue.AddLast(graph[firstRep[a].x, firstRep[a].y]);
            }
        }
        queue.RemoveFirst();

        while (queue.Any())
        {
            if (layerProgress > 0 && layerProgress % (6 * LayerMultiplier) == 0)
            {
                value++;
                LayerMultiplier++;
                layerProgress = 0;
            }

            if (value > 4)
            {
                return;
            }

            //dequeue a vertex from the queue and print it
            Node S = queue.First();
            //This is where I do stuff, I guess?
            ProximityGrid[S.x, S.y] = value;
            layerProgress++;
            List<Node> list = S.neighbours;

            for (int a = 0; a < list.Count; a++)
            {
                if (!visited[list[a].x, list[a].y])
                {
                    visited[list[a].x, list[a].y] = true;
                    queue.AddLast(graph[list[a].x, list[a].y]);
                }
            }
            queue.RemoveFirst();
        }
    }

    #endregion

    private void Awake()
    {
        width = 120;
        height = 60;
        PercentAreWalls = 47;

        RandomFillMap();
    }
}
