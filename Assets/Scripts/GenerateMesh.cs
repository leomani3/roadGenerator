using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMesh : MonoBehaviour
{
    float[,] map;

    //hauteurs des "montagnes"
    public int depth;
    //largeur des "montagnes"
    public int scale;
    public int width;
    public int height;

    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        map = new float[width, height];  
        GenerateMap();
    }

   TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateMap());

        return terrainData;
    }

    float [,] GenerateMap()
    {
        float[,] m = new float[width, height];
        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                m[x, y] = CalculPerlinNoise(x, y);
            }
        }
        map = m;
        return m;
    }

    float CalculPerlinNoise(int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
