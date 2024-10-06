using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private int height;
    private int width;
    public GameObject tilePrefab;
    private GameObject[,] backgroundTile;
    public void SetupBoard()
    {
        height = ManagerConfig.ConfigBoard.height;
        width = ManagerConfig.ConfigBoard.width;
        backgroundTile = new GameObject[width, height];
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                backgroundTile[i,j] = Instantiate(tilePrefab, new Vector2(i, j), Quaternion.identity);
                backgroundTile[i, j].transform.SetParent(transform);
            }
        }

    }
}
