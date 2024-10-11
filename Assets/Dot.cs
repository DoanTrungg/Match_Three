using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Dot : MonoBehaviour
{
    private BackgroundTile _backgroundTile;
    private ID _id;
    public BackgroundTile BackgroundTile { get => _backgroundTile; set => _backgroundTile = value; }
    public ID Id { get => _id; set => _id = value; }
}
public enum ID
{
    Element0,
    Element1,
    Element2,
    Element3,
    Element4,
    Element5,
    Element6,
    Element7,
}
