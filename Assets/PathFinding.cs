﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class PathFinding : MonoBehaviour
{
    public int width;
    public int height;

    private Vector3Int currentStartPoint;
    private Vector3Int currentEndPoint;
    private bool startPointDefined;
    private bool endPointDefined;

    // Start is called before the first frame update
    void Start()
    {
        startPointDefined = false;
        endPointDefined = false;

    }

    // Update is called once per frame
    void Update()
    {
    }

    public List<Vector2Int> Dijsktra(Node[,] graph, Vector2Int startPoint, Vector2Int endPoint)
    {
        //initialisation
        Node startNode = new Node(startPoint.x, startPoint.y, 0, 0);
        Node endNode = new Node(endPoint.x, endPoint.y, 0, 0);

        List<Vector2Int> res = new List<Vector2Int>();

        List<Node> explored = new List<Node>();
        List<Node> chosen = new List<Node>();

        Node currentNode = startNode;

        //TODO : Bien initialiser les nodes avec l'attribut distance "1" en fonction de la pente
        /*for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                graph[i, j] = new Node(i, j, -1, 1);
            }
        }*/
        graph[startPoint.x, startPoint.y] = startNode;

        //exploration
        while (!currentNode.Equal(endNode))
        {
            //on étend aux voisins
            exploreNeighbors(currentNode, graph, explored, startNode, endNode);
            //on tri explored dans l'ordre croissant des couts
            explored.Sort(SortByCost);
            //choisir le plus bas cout parmis les noeuds explorés. par défaut explored[0] grace au tri.
            //Debug.Log(explored.Count);
            currentNode = explored[0];
            explored.RemoveAt(0);
        }
        //calcul du chemin
        while (!currentNode.Equal(startNode))
        {
            res.Add(new Vector2Int(currentNode.x, currentNode.y));
            currentNode = currentNode.parent;
        }

        return res;
    }

    static int SortByCost(Node n1, Node n2)
    {
        return n1.cost.CompareTo(n2.cost);
    }

    public void addNeighbor(Node currentNode, int neighborX, int neighborY, Node[,] graph, List<Node> explored, Node startNode, Node endNode)
    {
        if (graph[neighborX, neighborY].cost == -1 || (graph[neighborX, neighborY].cost != -1 && currentNode.cost + graph[neighborX, neighborY].distance < graph[neighborX, neighborY].cost))
        {
            graph[neighborX, neighborY].cost = currentNode.cost + graph[neighborX, neighborY].distance;
            graph[neighborX, neighborY].parent = currentNode;
            explored.Add(graph[neighborX, neighborY]);
        }
    }


    public void exploreNeighbors(Node currentNode, Node[,] graph, List<Node> explored, Node startNode, Node endNode)
    {
        if (currentNode.x > 0)
        {
            addNeighbor(currentNode, currentNode.x - 1, currentNode.y, graph, explored, startNode, endNode);
            if (currentNode.y > 0)
            {
                addNeighbor(currentNode, currentNode.x - 1, currentNode.y - 1, graph, explored, startNode, endNode);
                addNeighbor(currentNode, currentNode.x, currentNode.y - 1, graph, explored, startNode, endNode);
            }
            if (currentNode.y < height - 1)
            {
                addNeighbor(currentNode, currentNode.x - 1, currentNode.y + 1, graph, explored, startNode, endNode);
                addNeighbor(currentNode, currentNode.x, currentNode.y + 1, graph, explored, startNode, endNode);
            }
        }

        if (currentNode.x < width - 1)
        {
            addNeighbor(currentNode, currentNode.x + 1, currentNode.y, graph, explored, startNode, endNode);
            if (currentNode.y > 0)
            {
                addNeighbor(currentNode, currentNode.x + 1, currentNode.y - 1, graph, explored, startNode, endNode);
                addNeighbor(currentNode, currentNode.x, currentNode.y - 1, graph, explored, startNode, endNode);
            }
            if (currentNode.y < height - 1)
            {
                addNeighbor(currentNode, currentNode.x + 1, currentNode.y + 1, graph, explored, startNode, endNode);
                addNeighbor(currentNode, currentNode.x, currentNode.y + 1, graph, explored, startNode, endNode);
            }
        }
    }
}
