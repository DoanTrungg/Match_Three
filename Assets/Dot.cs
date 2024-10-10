using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Dot : MonoBehaviour
{
    private BackgroundTile _backgroundTile;
    
    public BackgroundTile BackgroundTile { get => _backgroundTile; set => _backgroundTile = value; }
   
}
