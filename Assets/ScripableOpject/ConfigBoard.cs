using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RemoteConfig", menuName = "Config/Board")]
public class ConfigBoard : ScriptableObject
{
    [Header("Board Size")]
    public int width;
    public int height;

    [Header("Dots Color")]
    public List<Color> listColor = new List<Color>();
}
