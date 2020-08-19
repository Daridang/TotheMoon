using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Rocket _rocket;
    private Rocket rocket;

    public GameObject[] systemPrefabs;

    private List<GameObject> _instancedSystemPrefabs;

    private string _currentLevelName = string.Empty;

    // List<AsyncOperation> _loadOperations;

    private void OnEnable()
    {
        
    }

    public string GetLevelName()
    {
        return SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        //_loadOperations = new List<AsyncOperation>();
        _instancedSystemPrefabs = new List<GameObject>();
        InstantiateSystemPrefabs();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //rocket = Instantiate(_rocket, _rocket.transform.localPosition, _rocket.transform.rotation);

        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        //for(int i = 0; i < _instancedSystemPrefabs.Count; i++)
        //{
        //    Destroy(_instancedSystemPrefabs[i]);
        //}
        //_instancedSystemPrefabs.Clear();
    }

    //public void LoadLevel(string levelName)
    //{
    //    AsyncOperation ao = SceneManager.LoadSceneAsync(levelName);
    //    if(ao == null)
    //    {
    //        return;
    //    }
    //    ao.completed += OnLoadOperationComplete;
    //    _loadOperations.Add(ao);

    //    _currentLevelName = levelName;
    //}

    //public void UnloadLevel(string levelName)
    //{
    //    AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
    //    if(ao == null)
    //    {
    //        return;
    //    }
    //    ao.completed += OnUnloadOperationComplete;
    //}


    //private void OnLoadOperationComplete(AsyncOperation ao)
    //{
    //    if(_loadOperations.Contains(ao))
    //    {
    //        _loadOperations.Remove(ao);
    //    }
    //    Debug.Log("Load complete.");
    //}

    //private void OnUnloadOperationComplete(AsyncOperation obj)
    //{
    //    Debug.Log("Unload complete.");
    //}

    private void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for(int i = 0; i < systemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(systemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    public void LoadLevel()
    {
        Initiate.Fade("Level1", Color.black, 1f);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
