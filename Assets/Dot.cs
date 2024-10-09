using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Dot : MonoBehaviour
{
    private Vector2 _firstTouchPosition;
    private Vector2 _finalTouchPosition;
    private float _swipeAngle;
    private float _swipeDistance;
    private BackgroundTile _backgroundTile;
    private Board _boad;
    

    public float SwipeAngle { get => _swipeAngle; set => _swipeAngle = value; }
    public BackgroundTile BackgroundTile { get => _backgroundTile; set => _backgroundTile = value; }
    public float SwipeDistance { get => _swipeDistance; set => _swipeDistance = value; }

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
        MovePieces((int)_swipeAngle, _swipeDistance);
        
    }
    private void CalculateAngle()
    {
        _swipeAngle = Mathf.Atan2(_finalTouchPosition.y - _firstTouchPosition.y, _finalTouchPosition.x - _firstTouchPosition.x) * 180 / Mathf.PI;
        _swipeDistance = Vector2.Distance(_firstTouchPosition, _finalTouchPosition);
        Debug.Log("angle : " + _swipeAngle + " - " + "distance : " + _swipeDistance);
    }
    private void MovePieces(int angle, float distance)
    {
        if (distance < 0.15f) return;
        var row = _backgroundTile.Row;
        var column = _backgroundTile.Column;
        if (angle <= 45 && angle > -45 && row < _boad.Width - 1)
        {
            // right swipe
            _boad.SwapDots(_boad.ListBackgroundTile[row, column], _boad.ListBackgroundTile[row + 1, column]);
        }
        else if (angle > 45 && angle <= 135 && column < _boad.Height - 1)
        {
            // up swipe
            _boad.SwapDots(_boad.ListBackgroundTile[row, column], _boad.ListBackgroundTile[row, column + 1]);
        }
        else if (angle > 135 || angle <= -135 && row > 0)
        {
            // left swipe
            _boad.SwapDots(_boad.ListBackgroundTile[row, column], _boad.ListBackgroundTile[row - 1, column]);
        }
        else if (angle < -45 && angle >= -135 && column > 0)
        {
            // down swipe
            _boad.SwapDots(_boad.ListBackgroundTile[row, column], _boad.ListBackgroundTile[row, column-1]);
        }
    }
}
