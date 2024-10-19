using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] private Dot _tilePrefab;
    [SerializeField] private GameObject _poolDot;
    private List<Dot> _listPool = new List<Dot>();

    public List<Dot> ListPool { get => _listPool; set => _listPool = value; }

    public void HideDot(Dot dot)
    {
        int row = dot.BackgroundTile.Row;
        int column = dot.BackgroundTile.Column;
        ListPool.Add(dot);
        dot.transform.SetParent(_poolDot.transform);
        dot.transform.position = Vector2.zero;
    }
   
}
