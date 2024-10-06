using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private Board board;
    private void Start()
    {
        board.SetupBoard();
    }
}
