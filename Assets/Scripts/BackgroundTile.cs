using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer imageDot;

    public void Initialized()
    {
        if (ManagerConfig.ConfigBoard.listColor.Count <= 0) return;
        imageDot.gameObject.SetActive(true);
        int random = Random.Range(0, ManagerConfig.ConfigBoard.listColor.Count);
        imageDot.color = ManagerConfig.ConfigBoard.listColor[random];
    }
}
