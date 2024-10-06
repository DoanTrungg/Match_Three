using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RemoteConfig", menuName = "Config/Board")]
public class ConfigBoard : ScriptableObject
{
    public int width;
    public int height;
}
