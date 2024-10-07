using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    private Vector2 _firstTouchPosition;
    private Vector2 _finalTouchPosition;
    private float _swipeAngle;
    private BackgroundTile _backgroundTile;
    private Board _boad;
    

    public float SwipeAngle { get => _swipeAngle; set => _swipeAngle = value; }
    public BackgroundTile BackgroundTile { get => _backgroundTile; set => _backgroundTile = value; }

    private void Awake()
    {
        _boad = Board.Instance.GetComponent<Board>();
    }

    private void OnMouseDown()
    {
        _firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseUp()
    {
        _finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
        MovePieces((int)_swipeAngle);
    }
    private void CalculateAngle()
    {
        _swipeAngle = Mathf.Atan2(_finalTouchPosition.y - _firstTouchPosition.y, _finalTouchPosition.x - _firstTouchPosition.x) * 180 / Mathf.PI;
        //Debug.Log(_swipeAngle);
    }
    private void MovePieces(int angle)
    {
        if(angle <= 45 && angle >= -45)
        {
            gameObject.transform.SetParent(_boad.ListBackgroundTile[_backgroundTile.Row+1, _backgroundTile.Column].GetComponent<Transform>());
            gameObject.transform.localPosition = Vector2.zero;
            _backgroundTile = _boad.ListBackgroundTile[_backgroundTile.Row+1, _backgroundTile.Column];
            Debug.Log(transform.parent.name + "nameee");
        }
    }

}
