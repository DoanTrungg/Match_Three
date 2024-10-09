using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundTile : MonoBehaviour
{
    [SerializeField] private Dot _dot;
    private int _column;
    private int _row;

    public int Column { get => _column; set => _column = value; }
    public int Row { get => _row; set => _row = value; }
    public Dot Dot { get => _dot; set => _dot = value; }
    private void Awake()
    {
        _dot = _dot.GetComponent<Dot>();
    }

    public void Initialize()
    {
        if (ManagerConfig.ConfigBoard.listColor.Count <= 0) return;
        _dot.gameObject.SetActive(true);
        int random = Random.Range(0, ManagerConfig.ConfigBoard.listColor.Count);
        _dot.GetComponent<SpriteRenderer>().color = ManagerConfig.ConfigBoard.listColor[random];
        _dot.GetComponent<Dot>().BackgroundTile = this;
    }
}
