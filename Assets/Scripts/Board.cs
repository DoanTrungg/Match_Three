using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private int height;
    private int width;
    private GameObject[,] backgroundTile;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform _cam;

    private void Start()
    {
        SetupBoard();
    }
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

        _cam.transform.position = new Vector3((float)height/2-0.5f,(float)width/2+0.5f,-10);

    }
}
