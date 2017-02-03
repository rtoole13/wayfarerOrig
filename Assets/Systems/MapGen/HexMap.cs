using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour {

    public GameObject hexPrefab;

    //Number of Hex tiles
    public int width = 20;
    public int height = 20;

    public string seed;

    public float xOffset = 0.882f;
    public float yOffset = 0.764f;

    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    private int[,] map;
    private MapPiece[,] objectMap;

    // Use this for initialization
    void Start () {
        
    }

    public MapPiece[,] GetNextMap()
    {
        GenerateMap();
        return objectMap;
    }

    private void GenerateMap()
    {
        map = new int[width, height];
        objectMap = new MapPiece[width, height];
        RandomFillMap();

        for (var i = 0; i < 5; i++)
        {
            SmoothMap();
        }

    }

    private GameObject CreatGameObject(int x,int y)
    {
        float xPos = x * xOffset;
        if (y % 2 == 1)
        {
            xPos += xOffset / 2f;
        }
        GameObject hex_go = (GameObject)Instantiate(hexPrefab, new Vector3(xPos, y * yOffset, 0), Quaternion.identity);
        hex_go.name = "Hex_" + x + "_" + y;
        hex_go.transform.SetParent(this.transform);
        MapPiece piece = hex_go.GetComponent<MapPiece>();
        objectMap[x, y] = piece;
        return hex_go;
    }

    private void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }
        System.Random rng = new System.Random(seed.GetHashCode());
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CreatGameObject(x, y);
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                    
                }
                else
                {
                    if(rng.Next(0, 100) < randomFillPercent)
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        map[x, y] = 0;
                        objectMap[x, y].SetWalkable();
                    }
                }
            }
        }

    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                if (neighbourWallTiles > 4)
                {
                    map[x, y] = 1;
                    objectMap[x, y].SetUnwalkable();
                }
                else if (neighbourWallTiles < 4)
                {
                    map[x, y] = 0;
                    objectMap[x, y].SetWalkable();
                }
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }
    
    /*
    void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.green : Color.blue;
                    Vector3 pos = new Vector3(x + .5f, y + .5f, 0);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
    */
}
