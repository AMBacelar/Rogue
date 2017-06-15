using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

    public GameObject floorHexPrefab;
    public GameObject wallHexPrefab;

    

    public class MapHandler
    {        
        //Size of the map in terms of hex tiles
        public int width;
        public int height;
        public int percentAreWalls;

        float xOffset = 0.939f;
        float zOffset = 0.814f;

        public int[,] hexGrid;        

        public void MakeCaverns()
        {
            for (int column = 0, row = 0; row <= height - 1; row++)
            {
                for (column = 0; column <= width - 1; column++)
                {
                    hexGrid[column, row] = PlaceWallLogic(column, row);
                }
            }
        }

        public int PlaceWallLogic(int x, int y)
        {
            int numWalls = GetAdjacentWalls(x, y, 1, 1);


            if (hexGrid[x, y] == 1)
            {
                if (numWalls >= 4)
                {
                    return 1;
                }
                if (numWalls < 2)
                {
                    return 0;
                }

            }
            else
            {
                if (numWalls >= 5)
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

            int wallCounter = 0;

            for (iY = startY; iY <= endY; iY++)
            {
                for (iX = startX; iX <= endX; iX++)
                {
                    if (!(iX == x && iY == y))
                    {
                        if (IsWall(iX, iY))
                        {
                            wallCounter += 1;
                        }
                    }
                }
            }
            return wallCounter;
        }

        bool IsWall(int x, int y)
        {
            // Consider out-of-bound a wall
            if (IsOutOfBounds(x, y))
            {
                return true;
            }

            if (hexGrid[x, y] == 1)
            {
                return true;
            }

            if (hexGrid[x, y] == 0)
            {
                return false;
            }
            return false;
        }

        bool IsOutOfBounds(int x, int y)
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

        public void loadMap(GameObject wallTile, GameObject floorTile, GameObject parent)
        {
            List<GameObject> tileType = new List<GameObject>();
            tileType.Add(floorTile);
            tileType.Add(wallTile);

            for (int column = 0, row = 0; row < height; row++)
            {
                for (column = 0; column < width; column++)
                {
                    float xPos = column * xOffset;
                    if (row % 2 == 1)
                    {
                        xPos += xOffset / 2f;
                    }

                    GameObject hex_go = (GameObject)Instantiate(tileType[hexGrid[column, row]], new Vector3(xPos, 0, row * zOffset), Quaternion.identity);

                    string tileName = "";

                    if(hexGrid[column,row] == 0)
                    {
                        tileName = "floorTile_";
                    } else if (hexGrid[column,row] == 1)
                    {
                        tileName = "wallTile_";
                    }

                    hex_go.name = tileName + "Hex_" + column + "_" + row;

                    hex_go.transform.SetParent(parent.transform);
                    
                    hex_go.isStatic = true;
                }
            }
        }

        public MapHandler(int mapWidth, int mapHeight, int[,] map, int percentWalls = 47)
        {
            this.width = mapWidth;
            this.height = mapHeight;
            this.percentAreWalls = percentWalls;
            this.hexGrid = map;
        }

        public MapHandler()
        {
            width = 120;
            height = 60;
            percentAreWalls = 47;

            hexGrid = new int[width, height];

            RandomFillMap();
        }
        public void BlankMap()
        {
            for (int column = 0, row = 0; row < height; row++)
            {
                for (column = 0; column < width; column++)
                {
                    hexGrid[column, row] = 0;
                }
            }
        }

        public void RandomFillMap()
        {
            // New, empty map
            hexGrid = new int[width, height];

            int mapMiddle = 0; // Temp variable
            for (int column = 0, row = 0; row < height; row++)
            {
                for (column = 0; column < width; column++)
                {
                    // If coordinants lie on the the edge of the map (creates a border)
                    if (column == 0)
                    {
                        hexGrid[column, row] = 1;
                    }
                    else if (row == 0)
                    {
                        hexGrid[column, row] = 1;
                    }
                    else if (column == width - 1)
                    {
                        hexGrid[column, row] = 1;
                    }
                    else if (row == height - 1)
                    {
                        hexGrid[column, row] = 1;
                    }
                    // Else, fill with a wall a random percent of the time
                    else
                    {
                        mapMiddle = (height / 2);

                        if (row == mapMiddle)
                        {
                            hexGrid[column, row] = 0;
                        }
                        else
                        {
                            hexGrid[column, row] = RandomPercent(percentAreWalls);
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

    // Use this for initialization
    void Start()
    {
        MapHandler Map = new MapHandler();

        Map.MakeCaverns();
        Map.loadMap(wallHexPrefab, floorHexPrefab, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
