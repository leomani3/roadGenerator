using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMesh : MonoBehaviour
{
    float[,] map;
    //Node[,] mapDijkstra;
    List<Vector2Int> result;
    //hauteurs des "montagnes"
    public int depth;
    //largeur des "montagnes"
    public int scale;
    public int width;
    public int height;
    public PathFinding dijkstra;
    public GameObject cube;
    public int nbSousRoutes;

    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        result = new List<Vector2Int>();
        map = new float[width, height];
        //mapDijkstra = new Node[width, height];
        dijkstra.width = width;
        dijkstra.height = height;
        //GenerateMap();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

   TerrainData GenerateTerrain(TerrainData terrainData)
    {
        //genere le bon nombre de données
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateMap());

        return terrainData;
    }

    float [,] GenerateMap()
    {
        Node[,] mapDijkstra = new Node[width, height];
        float[,] m = new float[width, height];
        dijkstra.height = height;
        dijkstra.width = width;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                m[x, y] = CalculPerlinNoise(x, y);
                if (m[x,y] * depth > 90.0f)
                {
                    mapDijkstra[x, y] = new Node(x, y, -1, 100000);
                }
                else
                {
                    mapDijkstra[x, y] = new Node(x, y, -1, Mathf.RoundToInt(Mathf.Pow(m[x, y] * depth, 1)));
                }
            }
        }

        map = m;



        //CALCUL DES ROUTES
        
        dijkstra.height = height;
        dijkstra.width = width;
        result = dijkstra.Dijsktra(mapDijkstra, new Vector2Int(0, 0), new Vector2Int(400, 300));
        for (int i = 0; i < result.Count; i++)
        {
            Instantiate(cube, new Vector3(result[i].x, map[result[i].y,result[i].x] * depth, result[i].y), Quaternion.identity);
        }

        //routes secondaires

       /* for (int i = 0; i < nbSousRoutes; i++)
        {
            List<Vector2Int> res = new List<Vector2Int>();

            res = dijkstra.Dijsktra(mapDijkstra, result[Random.Range(0, result.Count)], new Vector2Int(Random.Range(5, 400), Random.Range(5, 400)));
            for (int j = 0; j < res.Count; j++)
            {
                Instantiate(cube, new Vector3(res[j].x, map[res[j].y, res[j].x] * depth, res[j].y), Quaternion.identity);
            }
        }*/

        return m;
    }

    float CalculPerlinNoise(int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }

    void createLittleRoad()
    {
        
    }
}
