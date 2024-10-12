using DG.Tweening;
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
        Dot tempDot = tileA.Dot;
        ID tempId = tileA.Dot.Id;

        tileA.Dot.transform.DOMove(tileB.transform.position, 0.4f).OnStart(() =>
        {
            tileA.Dot.transform.SetParent(tileB.transform);
            tileA.Dot.transform.localPosition = Vector3.zero;
            tileA.Dot = tileB.Dot; // update parent
            tileA.Dot.Id = tileB.Dot.Id; // update property
            if (tileA.Dot != null) tileA.Dot.BackgroundTile = tileA; // update children

        }).OnComplete(() => FindMidMatches(tileA.Dot));

        tileB.Dot.transform.DOMove(tileA.transform.position, 0.4f).OnStart(() =>
        {
            tileB.Dot.transform.SetParent(tileA.transform);
            tileB.Dot.transform.localPosition = Vector3.zero;
            tileB.Dot = tempDot; // update paretn
            tileB.Dot.Id = tempId; // update property
            if (tileB.Dot != null) tileB.Dot.BackgroundTile = tileB; // update children 

        }).OnComplete(() => FindMidMatches(tileB.Dot));
    }
    public void FindMidMatches(Dot dot)
    {
        int currentRow = dot.BackgroundTile.Row;
        int currentColumn = dot.BackgroundTile.Column;

        if (currentRow > 0 && currentRow < Width - 1) // 0 x 0 - Width
        {
            int nextRow = dot.BackgroundTile.Row + 1;
            int previousRow = dot.BackgroundTile.Row - 1;
            if (dot.Id == ID.None || ListBackgroundTile[nextRow, currentColumn].Dot.Id == ID.None || ListBackgroundTile[previousRow, currentColumn].Dot.Id == ID.None) return;
            if (dot.Id == ListBackgroundTile[nextRow, currentColumn].Dot.Id &&
               dot.Id == ListBackgroundTile[previousRow, currentColumn].Dot.Id)
            {
                Matched(dot);
                Matched(ListBackgroundTile[nextRow, currentColumn].Dot);
                Matched(ListBackgroundTile[previousRow, currentColumn].Dot);
            }
        }
        if (currentColumn > 0 && currentColumn < Height - 1) // 0 x 0 - Height
        {
            int nextColumn = dot.BackgroundTile.Column + 1;
            int previousColumn = dot.BackgroundTile.Column - 1;
            if (dot.Id == ID.None || ListBackgroundTile[currentRow, nextColumn].Dot.Id == ID.None || ListBackgroundTile[currentRow, previousColumn].Dot.Id == ID.None) return;
            if (dot.Id == ListBackgroundTile[currentRow, nextColumn].Dot.Id &&
               dot.Id == ListBackgroundTile[currentRow, previousColumn].Dot.Id)
            {
                Matched(dot);
                Matched(ListBackgroundTile[currentRow, nextColumn].Dot);
                Matched(ListBackgroundTile[currentRow, previousColumn].Dot);
            }
        }

    }
    private void Matched(Dot dot)
    {
        dot.Matched = true;
        dot.GetComponent<SpriteRenderer>().color = Color.gray;
        dot.Id = ID.None;
    }
}
