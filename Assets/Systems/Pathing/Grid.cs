using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Grid : MonoBehaviour {

    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public float nodeRadius;
    public Node[,] grid;

    public Transform player;


    private HexMap mapGenerator;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    void Awake()
    {
        mapGenerator = GetComponent<HexMap>();
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(mapGenerator.width / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(mapGenerator.height / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        MapPiece[,] map = mapGenerator.GetNextMap();
        Vector3 worldBottomLeft = transform.position - Vector3.right * mapGenerator.width / 2 - Vector3.up * mapGenerator.height / 2;
        for (int x = 0;x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = Vector3.right * (map[x, y].GetX() * nodeDiameter + nodeRadius) + Vector3.up *(map[x, y].GetY() * nodeDiameter +nodeRadius);
                grid[x, y] = new Node(map[x, y].walkable, worldPoint, x,y);
            }
        }
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateGrid();
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for(int x = -1; x<=1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if(checkX >= 0 && checkX < gridSizeX && checkY >=0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x / 0.882f) / mapGenerator.width;
        float percentY = (worldPosition.y / 0.764f) / mapGenerator.height;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }
    
    void OnDrawGizmos()
    {
        if(grid != null && displayGridGizmos)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(mapGenerator.width, mapGenerator.height, 1));

            Node playerNode = NodeFromWorldPoint(player.position);

            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if(playerNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter));
            }
        }
    }
    
}
