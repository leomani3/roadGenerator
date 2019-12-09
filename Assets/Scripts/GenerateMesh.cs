﻿using System.Collections;
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

    public GameObject cube;

    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        map = new float[width, height];  
        GenerateMap();
        Instantiate(cube, new Vector3(511, map[511, 511]*depth , 511), Quaternion.identity);
        Instantiate(cube, new Vector3(501, map[501, 501] * depth, 501), Quaternion.identity);
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