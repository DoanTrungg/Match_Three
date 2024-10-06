using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/ManagerConfig")]
public class ManagerConfig : SingletonScriptableObject<ManagerConfig>
{
    [SerializeField] private ConfigBoard _configBoard;
    public static ConfigBoard ConfigBoard { get { return Instance._configBoard; } }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void FirstInitalizr()
    {
        Debug.Log("This message will ouput before Awake.");
    }
}
