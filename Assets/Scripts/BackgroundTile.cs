using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer imageDot;

    public SpriteRenderer ImageDot { get => imageDot; set => imageDot = value; }

    public void Initialize()
    {
        if (ManagerConfig.ConfigBoard.listColor.Count <= 0) return;
        ImageDot.gameObject.SetActive(true);
        int random = Random.Range(0, ManagerConfig.ConfigBoard.listColor.Count);
        ImageDot.color = ManagerConfig.ConfigBoard.listColor[random];
    }
}
