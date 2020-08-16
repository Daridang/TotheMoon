﻿using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; }

    public static bool IsInitialized { get { return Instance != null; } }

    protected virtual void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("Trying to instantiate a second instance.");
        }
        else
        {
            Instance = this as T;
        }
    }

    protected virtual void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }
}
