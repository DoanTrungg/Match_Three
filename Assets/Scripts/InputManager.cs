using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Dot _dot;
    private Direction direction;
    private Vector2 startPos;
    private Vector2 endPos; 
    private Vector2 difference;
    public float swipeThreshold = 100f;
    private Board _board;

    private void Awake()
    {
        direction = Direction.None;
        _dot = GetComponent<Dot>();
        _board = Board.Instance.GetComponent<Board>();
    }

    private void OnMouseDown()
    {
        startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseUp()
    {
        endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Swap();
    }
    private Direction SwipegGesture()
    {
        difference = endPos - startPos;
        if( difference.magnitude > swipeThreshold)
        {
            if(Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
            {
                return direction = difference.x > 0 ? Direction.Right : Direction.Left;
            }
            else
            {
                return direction = difference.y > 0 ? Direction.Up : Direction.Down;
            }
        }else return Direction.None;
    }
    private void Swap()
    {
        int row = _dot.BackgroundTile.Row;
        int column = _dot.BackgroundTile.Column;
        Direction direction = SwipegGesture();
        switch(direction)
        {
            case Direction.Right:
                if (row < _board.Width - 1) _board.SwapDots(_board.ListBackgroundTile[row, column], _board.ListBackgroundTile[row + 1, column]);
                break;
            case Direction.Left:
                if (row > 0) _board.SwapDots(_board.ListBackgroundTile[row, column], _board.ListBackgroundTile[row - 1, column]);
                break;
            case Direction.Up:
                if (column < _board.Height - 1) _board.SwapDots(_board.ListBackgroundTile[row, column], _board.ListBackgroundTile[row, column + 1]);
                break;
            case Direction.Down:
                if(column > 0) _board.SwapDots(_board.ListBackgroundTile[row, column], _board.ListBackgroundTile[row, column - 1]);
                break;
            default:
                Debug.Log("Dont Swap");
                break;
        }
    }
    

}
public enum Direction 
{ 
    Left, 
    Up, 
    Right, 
    Down, 
    None 
}
