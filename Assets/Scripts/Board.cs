using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private int height;
    private int width;
    private BackgroundTile[,] _listBackgroundTile;
    [SerializeField] private BackgroundTile tilePrefab;
    [SerializeField] private Transform _cam;
    public static Board Instance { get; private set; }
    public BackgroundTile[,] ListBackgroundTile { get => _listBackgroundTile; set => _listBackgroundTile = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogError("Multiple instances of Board detected! Stopping execution.");
            Debug.Break();
        }
    }

    private void Start()
    {
        SetupBoard();
    }
    public void SetupBoard()
    {
        height = ManagerConfig.ConfigBoard.height;
        width = ManagerConfig.ConfigBoard.width;
        ListBackgroundTile = new BackgroundTile[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                ListBackgroundTile[i, j] = Instantiate(tilePrefab, new Vector2(i, j), Quaternion.identity);
                SetBackgroundTile(ListBackgroundTile[i, j], i, j, transform);
            }
        }

        _cam.transform.position = new Vector3((float)height / 2 - 0.5f, (float)width / 2 + 0.5f, -10);

    }

    private void SetBackgroundTile(BackgroundTile backgroundTile, int width, int height, Transform transform)
    {
        backgroundTile.Initialize();
        backgroundTile.gameObject.name = "(" + width + "," + height + ")";
        backgroundTile.Row = width;
        backgroundTile.Column = height;
        backgroundTile.transform.SetParent(transform);
    }
}
