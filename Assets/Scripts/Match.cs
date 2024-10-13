using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Match : MonoBehaviour
{

    private Board _board;
    private int _width;
    private int _height;
    int countMatch;
    private List<Dot> _listMatched = new List<Dot>();
    private void Awake()
    {
        //_board = Board.Instance;
    }
    private void Start()
    {
        _board = Board.Instance; 
        if (_board == null)
        {
            Debug.LogError("Board instance is null!");
            return;
        }
        countMatch = 0;
        _width = ManagerConfig.ConfigBoard.width;
        _height = ManagerConfig.ConfigBoard.height;
    }

    public void FindMidMatches(Dot dot)
    {
        int currentRow = dot.BackgroundTile.Row;
        int currentColumn = dot.BackgroundTile.Column;
        
        MatchRightRow(dot, currentRow, currentColumn);
        MatchLeftRow(dot, currentRow, currentColumn);
        MatchHeadColumn(dot,currentRow, currentColumn);
        MatchTailColumn(dot, currentRow, currentColumn);
        Matched(_listMatched);
    }
    private void MatchRightRow(Dot dot, int currentRow, int currentColumn)
    {
        int minRow = currentRow == 0 ? 1 : -1;
        if (UnMatching(dot, _board.ListBackgroundTile[currentRow + (minRow == 1 ? 0 : -1), currentColumn].Dot, true))
        {
            if (MatchingRowColumn(dot, MatchStatus.RIGHT_ROW) == 2)
            {
                for (int rowMatch = 0; rowMatch < 3; rowMatch++)
                {
                    //Matched(_board.ListBackgroundTile[currentRow + rowMatch, currentColumn].Dot);
                    _listMatched.Add(_board.ListBackgroundTile[currentRow + rowMatch, currentColumn].Dot);
                }
                countMatch = 0;
            }
        }
    }
    private void MatchLeftRow(Dot dot, int currentRow, int currentColumn)
    {
        int maxRow = currentRow == _width - 1 ? 1 : -1;
        if (UnMatching(dot, _board.ListBackgroundTile[currentRow + (maxRow == 1 ? 0 : 1), currentColumn].Dot, true)) // Left_Row
        {
            if (MatchingRowColumn(dot, MatchStatus.LEFT_ROW) == 2)
            {
                for (int rowMatch = 0; rowMatch < 3; rowMatch++)
                {
                    //Matched(_board.ListBackgroundTile[currentRow - rowMatch, currentColumn].Dot);
                    _listMatched.Add(_board.ListBackgroundTile[currentRow - rowMatch, currentColumn].Dot);
                }
                countMatch = 0;
            }
        }
    }
    private void MatchHeadColumn(Dot dot, int currentRow, int currentColumn)
    {
        int minColumn = currentColumn == 0 ? 1 : -1;
        if (UnMatching(dot, _board.ListBackgroundTile[currentRow, currentColumn + (minColumn == 1 ? 0 : -1)].Dot, false))
        {
            if (MatchingRowColumn(dot, MatchStatus.HEAD_COLUMN) == 2)
            {
                for (int columnMatch = 0; columnMatch < 3; columnMatch++)
                {
                    //Matched(_board.ListBackgroundTile[currentRow, currentColumn + columnMatch].Dot);
                    _listMatched.Add(_board.ListBackgroundTile[currentRow, currentColumn + columnMatch].Dot);
                }
                countMatch = 0;
            }
        }
    }
    private void MatchTailColumn(Dot dot,int currentRow, int currentColumn)
    {
        int maxColumn = currentColumn == _height - 1 ? 1 : -1;
        if (UnMatching(dot, _board.ListBackgroundTile[currentRow, currentColumn + (maxColumn == 1 ? 0 : 1)].Dot, false))
        {
            if (MatchingRowColumn(dot, MatchStatus.TAIL_COLUMN) == 2)
            {
                for (int columnMatch = 0; columnMatch < 3; columnMatch++)
                {
                    //Matched(_board.ListBackgroundTile[currentRow, currentColumn - columnMatch].Dot);
                    _listMatched.Add(_board.ListBackgroundTile[currentRow, currentColumn - columnMatch].Dot);
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
            if (currentDot.Id == newDot.Id) return true;

            if ((newDot.BackgroundTile.Row >= 0 || newDot.BackgroundTile.Row < _width) && newDot.Id != idCurrentDot) return true;

            return false;
        }
        else
        {

            if (currentDot.Id == newDot.Id) return true;

            if ((newDot.BackgroundTile.Column >= 0 || newDot.BackgroundTile.Column < _height) && newDot.Id != idCurrentDot) return true;

            return false;
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
                    if (dot.BackgroundTile.Row + nextRow < _width && _board.ListBackgroundTile[dot.BackgroundTile.Row + nextRow, dot.BackgroundTile.Column].Dot.Id == currentId)
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
                    if (dot.BackgroundTile.Row - previousRow >= 0 && _board.ListBackgroundTile[dot.BackgroundTile.Row - previousRow, dot.BackgroundTile.Column].Dot.Id == currentId)
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
            case MatchStatus.HEAD_COLUMN:
                for(int nextColum = 1; nextColum < 3; nextColum++)
                {
                    if(dot.BackgroundTile.Column + nextColum < _height && _board.ListBackgroundTile[dot.BackgroundTile.Row, dot.BackgroundTile.Column + nextColum].Dot.Id == currentId)
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
            case MatchStatus.TAIL_COLUMN:
                for(int previous = 1; previous < 3; previous++)
                {
                    if(dot.BackgroundTile.Column - previous >= 0 && _board.ListBackgroundTile[dot.BackgroundTile.Row, dot.BackgroundTile.Column - previous].Dot.Id == currentId)
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
    private void Matched(List<Dot> listMatched)
    {
        if (listMatched.Count < 3 || listMatched == null) return;
        foreach(Dot dot in listMatched)
        {
            dot.Matched = true;
            dot.GetComponent<SpriteRenderer>().color = Color.gray;
            dot.Id = ID.None;
        }
        listMatched.Clear();
    }
}
public enum MatchStatus
{
    RIGHT_ROW,
    LEFT_ROW,
    MID_ROW,

    HEAD_COLUMN,
    MID_COLUMN,
    TAIL_COLUMN,
}
