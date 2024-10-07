using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundTile : MonoBehaviour
{
    [SerializeField] private Dot dot;


    public void Initialize()
    {
        if (ManagerConfig.ConfigBoard.listColor.Count <= 0) return;
        dot.gameObject.SetActive(true);
        int random = Random.Range(0, ManagerConfig.ConfigBoard.listColor.Count);
        dot.SpriteRenderer.color = ManagerConfig.ConfigBoard.listColor[random];
    }
}
