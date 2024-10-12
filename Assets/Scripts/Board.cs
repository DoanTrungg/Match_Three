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

    int countMatch;

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
        countMatch = 0;
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

        if (UnMatching(dot, ListBackgroundTile[currentRow - 1, currentColumn].Dot, true)) // Right_Row
        {
            if (MatchingRowColumn(dot, MatchStatus.RIGHT_ROW) == 2)
            {
                for (int rowMatch = 0; rowMatch < 3; rowMatch++)
                {
                    Matched(ListBackgroundTile[currentRow + rowMatch, currentColumn].Dot);
                }
                countMatch = 0;
            }
        }
        if (UnMatching(dot, ListBackgroundTile[currentRow + 1, currentColumn].Dot, true)) // Left_Row
        {
            if (MatchingRowColumn(dot, MatchStatus.LEFT_ROW) == 2)
            {
                for (int rowMatch = 0; rowMatch < 3; rowMatch++)
                {
                    Matched(ListBackgroundTile[currentRow - rowMatch, currentColumn].Dot);
                }
                countMatch = 0;
            }
        }

    }
    private bool UnMatching(Dot currentDot, Dot newDot, bool row)
    {
        ID idCurrentDot = currentDot.Id;
        if (row)
        {
            if ((newDot.BackgroundTile.Row >= 0 || newDot.BackgroundTile.Row < Width) && newDot.Id != idCurrentDot)
            {
                return true;
            }
            else return false;
        }
        else
        {
            if ((newDot.BackgroundTile.Column >= 0 || newDot.BackgroundTile.Column < Height) && newDot.Id != idCurrentDot)
            {
                return true;
            }
            else return false;
        }
    }
    private int MatchingRowColumn(Dot dot, MatchStatus matchStatus)
    {
        ID currentId = dot.Id;
        switch (matchStatus)
        {
            case MatchStatus.RIGHT_ROW:
                for (int nextRow = 1; nextRow < 3; nextRow++)
                { 
                    if (dot.BackgroundTile.Row + nextRow < Width && ListBackgroundTile[dot.BackgroundTile.Row + nextRow, dot.BackgroundTile.Column].Dot.Id == currentId)
                    {
                        countMatch++;
                    }
                    else
                    {
                        countMatch = 0;
                        break;
                    }
                }
                break;
            case MatchStatus.LEFT_ROW:
                for (int previousRow = 1; previousRow < 3; previousRow++)
                {
                    if (dot.BackgroundTile.Row - previousRow >= 0 && ListBackgroundTile[dot.BackgroundTile.Row - previousRow, dot.BackgroundTile.Column].Dot.Id == currentId)
                    {
                        countMatch++;
                    }
                    else
                    {
                        countMatch = 0;
                        break;
                    }
                }
                break;
            default: 
                break;
        }
        return countMatch;
    }
    private void Matched(Dot dot)
    {
        dot.Matched = true;
        dot.GetComponent<SpriteRenderer>().color = Color.gray;
        dot.Id = ID.None;
    }
}
