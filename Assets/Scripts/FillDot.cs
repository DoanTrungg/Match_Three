using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class FillDot : MonoBehaviour
{
    private Board _board;
    private PoolManager _poolManager;
    private List<Dot> _listDotExist = new List<Dot>();

    public List<Dot> ListDotExist { get => _listDotExist; set => _listDotExist = value; }

    private void Start()
    {
        _board = Board.Instance();
        _poolManager = PoolManager.Instance();
    }
    private List<Dot> DotsExist(Dot dot)
    {

        int currentRow = dot.BackgroundTile.Row;
        int currentColumn = dot.BackgroundTile.Column;

        int upColum = (_board.Height - 1) - currentColumn;
        for(int index = 1; index <= upColum; index++)
        {
            if(currentColumn + index < _board.Height && _board.ListBackgroundTile[currentRow, currentColumn + index].Dot.Id != _board.ListBackgroundTile[currentRow, currentColumn].Dot.Id)
            {
                ListDotExist.Add(_board.ListBackgroundTile[currentRow, currentColumn + index].Dot);
            }
        }
        return ListDotExist;
    }
    public void Fill(Dot dot)
    {
        List<Dot> list = DotsExist(dot);
        Debug.Log(list.Count + "counttt");
        foreach (var item in list)
        {
            int row = item.BackgroundTile.Row;
            int column = item.BackgroundTile.Column;
            item.transform.DOMove(_board.ListBackgroundTile[row, column - 1].transform.position, 0.4f);
            _board.UpdateInfor(item.BackgroundTile, _board.ListBackgroundTile[row, column - 1]);
        }
        list.Clear();

    }
    public void GetNewDot(Dot dot)
    {
        Dot newDot = _poolManager.ListPool[0];
        _poolManager.ListPool.Remove(newDot);

        int random = Random.Range(0, ManagerConfig.ConfigBoard.listColor.Count);
        int row = newDot.BackgroundTile.Row;
        newDot.FadeIn(0.4f).OnStart(() =>
        {
            newDot.transform.SetParent(_board.ListBackgroundTile[dot.BackgroundTile.Row, _board.Height - 1].transform);
            newDot.transform.localPosition = Vector2.zero;

            newDot.GetComponent<SpriteRenderer>().color = ManagerConfig.ConfigBoard.listColor[random];
            newDot.GetComponent<Dot>().BackgroundTile = _board.ListBackgroundTile[dot.BackgroundTile.Row, _board.Height - 1];
            newDot.GetComponent<Dot>().Id = (ID)random;
            newDot.Matched = false;

            _board.UpdateInfor(newDot.BackgroundTile, _board.ListBackgroundTile[dot.BackgroundTile.Row, _board.Height - 1]);
        });
        

    }

}
