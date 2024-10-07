using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private int height;
    private int width;
    private BackgroundTile[,] backgroundTile;
    [SerializeField] private BackgroundTile tilePrefab;
    [SerializeField] private Transform _cam;

    private void Start()
    {
        SetupBoard();
    }
    public void SetupBoard()
    {
        height = ManagerConfig.ConfigBoard.height;
        width = ManagerConfig.ConfigBoard.width;
        backgroundTile = new BackgroundTile[width, height];
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                backgroundTile[i,j] = Instantiate(tilePrefab, new Vector2(i, j), Quaternion.identity);
                backgroundTile[i,j].Initialize();
                backgroundTile[i,j].gameObject.name = "(" + i + "," + j + ")";
                backgroundTile[i,j].transform.SetParent(transform);
            }
        }

        _cam.transform.position = new Vector3((float)height/2-0.5f,(float)width/2+0.5f,-10);

    }
}
