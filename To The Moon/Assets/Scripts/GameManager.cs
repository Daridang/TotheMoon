using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Rocket _rocket;
    private Rocket rocket;

    public GameObject[] systemPrefabs;

    private List<GameObject> _instancedSystemPrefabs;

    private string _currentLevelName = string.Empty;

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
