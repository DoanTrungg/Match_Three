using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board : Singleton<Board>
{
    private int _height;
    private int _width;
    private BackgroundTile[,] _listBackgroundTile;
    [SerializeField] private BackgroundTile tilePrefab;
    [SerializeField] private Transform _cam;
    [SerializeField] private Match match;
    //public static Board Instance { get; private set; }
    public BackgroundTile[,] ListBackgroundTile { get => _listBackgroundTile; set => _listBackgroundTile = value; }
    public int Height { get => _height; set => _height = value; }
    public int Width { get => _width; set => _width = value; }

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

        }).OnComplete(() => match.FindMidMatches(tileA.Dot));

        tileB.Dot.transform.DOMove(tileA.transform.position, 0.4f).OnStart(() =>
        {
            tileB.Dot.transform.SetParent(tileA.transform);
            tileB.Dot.transform.localPosition = Vector3.zero;
            tileB.Dot = tempDot; // update paretn
            tileB.Dot.Id = tempId; // update property
            if (tileB.Dot != null) tileB.Dot.BackgroundTile = tileB; // update children 

        }).OnComplete(() => match.FindMidMatches(tileB.Dot));
    }
    
}
