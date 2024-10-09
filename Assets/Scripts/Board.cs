using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private int _height;
    private int _width;
    private BackgroundTile[,] _listBackgroundTile;
    [SerializeField] private BackgroundTile tilePrefab;
    [SerializeField] private Transform _cam;
    public static Board Instance { get; private set; }
    public BackgroundTile[,] ListBackgroundTile { get => _listBackgroundTile; set => _listBackgroundTile = value; }
    public int Height { get => _height; set => _height = value; }
    public int Width { get => _width; set => _width = value; }

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
        Height = ManagerConfig.ConfigBoard.height;
        Width = ManagerConfig.ConfigBoard.width;
        ListBackgroundTile = new BackgroundTile[Width, Height];
        SetupBoard();
    }
    public void SetupBoard()
    {
        
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                ListBackgroundTile[i, j] = Instantiate(tilePrefab, new Vector2(i, j), Quaternion.identity);
                SetBackgroundTile(ListBackgroundTile[i, j], i, j, transform);
            }
        }

        _cam.transform.position = new Vector3((float)Width / 2 - 0.5f, (float)Width / 2 + 0.5f, -10);

    }

    private void SetBackgroundTile(BackgroundTile backgroundTile, int width, int height, Transform transform)
    {
        backgroundTile.Initialize();
        backgroundTile.gameObject.name = "(" + width + "," + height + ")";
        backgroundTile.Row = width;
        backgroundTile.Column = height;
        backgroundTile.transform.SetParent(transform);
    }
    public void SwapDots(BackgroundTile tileA, BackgroundTile tileB)
    {
        tileA.Dot.transform.SetParent(tileB.transform);
        tileA.Dot.transform.localPosition = Vector3.zero;

        tileB.Dot.transform.SetParent(tileA.transform);
        tileB.Dot.transform.localPosition = Vector3.zero;

        Dot tempDot = tileA.Dot;
        tileA.Dot = tileB.Dot;
        tileB.Dot = tempDot;

        if (tileA.Dot != null) tileA.Dot.BackgroundTile = tileA;
        if (tileB.Dot != null) tileB.Dot.BackgroundTile = tileB;

    }
}
