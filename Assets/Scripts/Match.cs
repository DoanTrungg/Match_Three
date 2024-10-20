using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Match : MonoBehaviour
{
    [SerializeField] private float durationFade;
    private Board _board;
    private PoolManager _poolManager;
    private int _width;
    private int _height;
    private int countMatch;
    private List<Dot> _listMatched = new List<Dot>();
    private void Start()
    {
        _board = Board.Instance();
        _poolManager = PoolManager.Instance();
        if (_board == null || _poolManager == null)
        {
            Debug.LogError("Board instance is null!");
            return;
        }
        countMatch = 0;
        _width = ManagerConfig.ConfigBoard.width;
        _height = ManagerConfig.ConfigBoard.height;
    }

    public void FindMatch(Dot dot)
    {
        bool matchRightRow = MatchRightRow(dot);
        bool matchLeftRow = MatchLeftRow(dot);
        bool matchHeadColumn = MatchHeadColumn(dot);
        bool matchTailColumn = MatchTailColumn(dot);

        MatchMidRow(matchRightRow, matchLeftRow, dot);
        MatchMidColum(matchHeadColumn, matchTailColumn, dot);

        Matched(_listMatched);
    }
    private void MatchMidColum(bool matchHead, bool matchTail, Dot dot)
    {
        int currentRow = dot.BackgroundTile.Row;
        int currentColumn = dot.BackgroundTile.Column;
        if (!matchHead && !matchTail)
        {
            int countMatchHead = MatchingRowColumn(dot, MatchStatus.HEAD_COLUMN);
            int countMatchTail = MatchingRowColumn(dot, MatchStatus.TAIL_COLUMN);
            if (countMatchTail > 0)
            {
                for (int columnMatch = countMatchTail; columnMatch >= 0; columnMatch--)
                {
                    _listMatched.Add(_board.ListBackgroundTile[currentRow, currentColumn - columnMatch].Dot);
                }
                countMatch = 0;
            }
            if (countMatchHead > 0)
            {
                for (int columnMatch = 1; columnMatch <= countMatchHead; columnMatch++)
                {
                    _listMatched.Add(_board.ListBackgroundTile[currentRow, currentColumn + columnMatch].Dot);
                }
                countMatch = 0;
            }



        }
    }
    private void MatchMidRow(bool matchRight, bool matchLeft, Dot dot)
    {
        int currentRow = dot.BackgroundTile.Row;
        int currentColumn = dot.BackgroundTile.Column;
        if (!matchRight && !matchLeft)
        {
            int countMatchRight = MatchingRowColumn(dot, MatchStatus.RIGHT_ROW);
            int countMatchLeft = MatchingRowColumn(dot, MatchStatus.LEFT_ROW);
            if (countMatchRight > 0)
            {
                for (int rowMatch = 0; rowMatch <= countMatchRight; rowMatch++)
                {
                    _listMatched.Add(_board.ListBackgroundTile[currentRow + rowMatch, currentColumn].Dot);
                }
                countMatch = 0;
            }
            if (countMatchLeft > 0)
            {
                for (int rowMatch = 0; rowMatch <= countMatchLeft; rowMatch++)
                {
                    _listMatched.Add(_board.ListBackgroundTile[currentRow - rowMatch, currentColumn].Dot);
                }
                countMatch = 0;
            }
        }
    }
    private bool MatchRightRow(Dot dot)
    {
        int currentRow = dot.BackgroundTile.Row;
        int currentColumn = dot.BackgroundTile.Column;
        int minRow = currentRow == 0 ? 1 : -1;
        if (UnMatching(dot, _board.ListBackgroundTile[currentRow + (minRow == 1 ? 0 : -1), currentColumn].Dot, true))
        {
            if (MatchingRowColumn(dot, MatchStatus.RIGHT_ROW) == 2)
            {
                for (int rowMatch = 0; rowMatch < 3; rowMatch++)
                {
                    _listMatched.Add(_board.ListBackgroundTile[currentRow + rowMatch, currentColumn].Dot);
                }
                countMatch = 0;
            }
            return true;
        }
        else return false;
    }
    private bool MatchLeftRow(Dot dot)
    {
        int currentRow = dot.BackgroundTile.Row;
        int currentColumn = dot.BackgroundTile.Column;
        int maxRow = currentRow == _width - 1 ? 1 : -1;
        if (UnMatching(dot, _board.ListBackgroundTile[currentRow + (maxRow == 1 ? 0 : 1), currentColumn].Dot, true)) // Left_Row
        {
            if (MatchingRowColumn(dot, MatchStatus.LEFT_ROW) == 2)
            {
                for (int rowMatch = 0; rowMatch < 3; rowMatch++)
                {
                    _listMatched.Add(_board.ListBackgroundTile[currentRow - rowMatch, currentColumn].Dot);
                }
                countMatch = 0;
            }
            return true;
        }
        else return false;
    }
    private bool MatchHeadColumn(Dot dot)
    {
        int currentRow = dot.BackgroundTile.Row;
        int currentColumn = dot.BackgroundTile.Column;
        int minColumn = currentColumn == 0 ? 1 : -1;
        if (UnMatching(dot, _board.ListBackgroundTile[currentRow, currentColumn + (minColumn == 1 ? 0 : -1)].Dot, false))
        {
            if (MatchingRowColumn(dot, MatchStatus.HEAD_COLUMN) == 2)
            {
                for (int columnMatch = 0; columnMatch < 3; columnMatch++)
                {
                    _listMatched.Add(_board.ListBackgroundTile[currentRow, currentColumn + columnMatch].Dot);
                }
                countMatch = 0;
            }
            return true;
        }
        else return false;
    }
    private bool MatchTailColumn(Dot dot)
    {
        int currentRow = dot.BackgroundTile.Row;
        int currentColumn = dot.BackgroundTile.Column;
        int maxColumn = currentColumn == _height - 1 ? 1 : -1;
        if (UnMatching(dot, _board.ListBackgroundTile[currentRow, currentColumn + (maxColumn == 1 ? 0 : 1)].Dot, false))
        {
            if (MatchingRowColumn(dot, MatchStatus.TAIL_COLUMN) == 2)
            {
                for (int columnMatch = 2; columnMatch >= 0; columnMatch--)
                {
                    _listMatched.Add(_board.ListBackgroundTile[currentRow, currentColumn - columnMatch].Dot);
                }
                countMatch = 0;
            }
            return true;
        }
        else return false;
    }
    private bool UnMatching(Dot currentDot, Dot newDot, bool row)
    {
        ID idCurrentDot = currentDot.Id;
        if (row)
        {
            if (currentDot.Id == newDot.Id && currentDot.BackgroundTile.Row == newDot.BackgroundTile.Row) return true;

            if ((newDot.BackgroundTile.Row >= 0 || newDot.BackgroundTile.Row < _width) && newDot.Id != idCurrentDot) return true;

            return false;
        }
        else
        {

            if (currentDot.Id == newDot.Id && currentDot.BackgroundTile.Column == newDot.BackgroundTile.Column) return true;

            if ((newDot.BackgroundTile.Column >= 0 || newDot.BackgroundTile.Column < _height) && newDot.Id != idCurrentDot) return true;

            return false;
        }
    }
    private int MatchingRowColumn(Dot dot, MatchStatus matchStatus)
    {
        ID currentId = dot.Id;
        int currentRow = dot.BackgroundTile.Row;
        int currentColumn = dot.BackgroundTile.Column;
        countMatch = 0;
        switch (matchStatus)
        {
            case MatchStatus.RIGHT_ROW:
                for (int nextRow = 1; nextRow < 3; nextRow++)
                {
                    if (currentRow + nextRow < _width && _board.ListBackgroundTile[currentRow + nextRow, currentColumn].Dot.Id == currentId)
                    {
                        countMatch++;
                    }

                }
                break;
            case MatchStatus.LEFT_ROW:
                for (int previousRow = 1; previousRow < 3; previousRow++)
                {
                    if (currentRow - previousRow >= 0 && _board.ListBackgroundTile[currentRow - previousRow, currentColumn].Dot.Id == currentId)
                    {
                        countMatch++;
                    }

                }
                break;
            case MatchStatus.HEAD_COLUMN:
                for (int nextColum = 1; nextColum < 3; nextColum++)
                {
                    if (currentColumn + nextColum < _height && _board.ListBackgroundTile[currentRow, currentColumn + nextColum].Dot.Id == currentId)
                    {
                        countMatch++;
                    }
                }
                break;
            case MatchStatus.TAIL_COLUMN:
                for (int previous = 1; previous < 3; previous++)
                {
                    if (currentColumn - previous >= 0 && _board.ListBackgroundTile[currentRow, currentColumn - previous].Dot.Id == currentId)
                    {
                        countMatch++;
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
        if (listMatched.Count < 3) return;
        for (int i = listMatched.Count - 1; i >= 0; i--)
        {
            Dot dot = listMatched[i];
            if (dot.Id == ID.None) continue;
            DestroyDot(dot).OnComplete(() =>
            {
                Sequence sequence = DOTween.Sequence();

                // Thêm _board.FillDot.Fill(dot) vào chuỗi (giả sử nó có thể trả về một Tween)
                sequence.AppendCallback(() => _board.FillDot.Fill(dot));

                // Thêm _poolManager.HideDot(dot) vào chuỗi
                sequence.AppendCallback(() => _poolManager.HideDot(dot));

                // Khi 2 hàm trên chạy xong, thêm hàm thứ ba
                sequence.OnComplete(() => 
                {
                    _board.FillDot.GetNewDot();
                });
            });
        }
        listMatched.Clear();

    }
    private Tween DestroyDot(Dot dot)
    {
        dot.Matched = true;
        dot.Id = ID.None;
        return dot.FadeOut(durationFade);
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
