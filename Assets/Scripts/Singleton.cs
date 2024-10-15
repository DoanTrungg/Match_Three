using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component 
{
    private static T _instance;
    public static T Instance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<T>();
            if( _instance == null ) 
            {
                GameObject ob = new GameObject();
                _instance = ob.AddComponent<T>();
            }
        }
        return _instance;
    }
    private void Awake()
    {
        _instance = this as T;
    }
}
