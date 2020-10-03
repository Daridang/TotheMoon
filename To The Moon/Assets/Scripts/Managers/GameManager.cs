using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Transform _playerSpawn;
    [SerializeField] private GameObject _levelGenerator;
    [SerializeField] private Transform _textPopUp;

    private string _currentLevelName = string.Empty;
    private int currentSceneIndex;
    private GameObject _generator;
    private GameObject _rocket;
    private bool isConnectedToNetwork = false;
    private List<string> _sceneNames;

    private List<Interactive> _interactiveObjects;

    public int StarsInCurrentLevel { get; set; }
    public int DeathCount { get; set; }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        _interactiveObjects = new List<Interactive>();
    }

    private void Update()
    {
        for (int i = 0; i < _interactiveObjects.Count; i++)
        {
            Interactive interactive = _interactiveObjects[i];
            if(interactive is ICollectable collectable)
            {
                collectable.CollectableAction();
            }

            if(interactive is IEnemy enemy)
            {
                enemy.EnemyAction();
            }
        }
    }

    public int GetLevelCount()
    {
        int sceneCountInBuildSettings = SceneManager.sceneCountInBuildSettings;
        _sceneNames = new List<string>();
        for (int i = 0; i < sceneCountInBuildSettings; i++)
        {
            //string name1 = SceneManager.GetSceneByBuildIndex(i).name;
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name1 = GetSceneNameFromScenePath(path);
            Debug.Log("wtf: " + name1 + "scenes: " + sceneCountInBuildSettings);
            if (name1.StartsWith("Level"))
            {
                _sceneNames.Add(name1);
            }
        }
        return _sceneNames.Count;
    }

    private static string GetSceneNameFromScenePath(string scenePath)
    {
        // Unity's asset paths always use '/' as a path separator
        var sceneNameStart = scenePath.LastIndexOf("/", StringComparison.Ordinal) + 1;
        var sceneNameEnd = scenePath.LastIndexOf(".", StringComparison.Ordinal);
        var sceneNameLength = sceneNameEnd - sceneNameStart;
        return scenePath.Substring(sceneNameStart, sceneNameLength);
    }

    public void GameOver()
    {
        StarsInCurrentLevel = 0;
        DeathCount++;
        SoundManager.Instance.PlayLoseMusic();
        UIManager.Instance.HideHUD();
        UIManager.Instance.ShowGameOver();
    }

    public void LoadMainMenu()
    {
        UIManager.Instance.LoadMainMenuScene();
    }

    public void AddEnemy(Interactive enemy)
    {
        enemy.OnDestroyObject += InteractiveObjectOnDestroy;
        _interactiveObjects.Add(enemy);
    }

    public void RemoveEnemy(Interactive enemy)
    {
        enemy.OnDestroyObject -= InteractiveObjectOnDestroy;
        _interactiveObjects.Remove(enemy);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _interactiveObjects.Clear();
        _interactiveObjects = FindObjectsOfType<Interactive>().ToList();

        foreach(var obj in _interactiveObjects)
        {
            obj.OnDestroyObject += InteractiveObjectOnDestroy;
        }

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (!scene.name.Equals("Main") && !scene.name.Equals("Shop") && !scene.name.Equals("SelectLevel") && !scene.name.Equals("Space"))
        {
            StarsInCurrentLevel = 0;
            UIManager.Instance.ToggleHUD(true);
            UIManager.Instance.HideGameOver();
            UIManager.Instance.ShowLevelCount();
            UIManager.Instance.EnergyProgress.fillAmount = 1f;
            int selectedRocketIndex = DataManager.Instance.SelectedRocketIndex;
            GameObject r = GetComponent<RocketsArray>().PlayerRocketPrefabs[selectedRocketIndex];
            r.transform.localPosition = new Vector3(0f, 1f, 0);
            _rocket = Instantiate(r, r.transform.localPosition, r.transform.rotation);
            //_rocket.GetComponent<Rocket>().RocketData.Acceleration = 0;
        }

        if (scene.name.Equals("Main"))
        {
            UIManager.Instance.HideGameOver();
        }

        if (scene.name.Equals("Space"))
        {
            LevelGenerator.Scripts.LevelGenerator generator = Instantiate(_levelGenerator.GetComponent<LevelGenerator.Scripts.LevelGenerator>(), new Vector3(0, 0, 0), Quaternion.identity);
            generator.Init();
            _generator = generator.gameObject;

            StarsInCurrentLevel = 0;
            UIManager.Instance.ToggleHUD(true);
            UIManager.Instance.HideGameOver();
            UIManager.Instance.EnergyProgress.fillAmount = 1f;
            int selectedRocketIndex = DataManager.Instance.SelectedRocketIndex;
            GameObject r = GetComponent<RocketsArray>().PlayerRocketPrefabs[selectedRocketIndex];
            r.transform.localPosition = new Vector3(0f, 1f, 0);
            _rocket = Instantiate(r, r.transform.localPosition, r.transform.rotation);
            _rocket.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private void InteractiveObjectOnDestroy(Interactive obj)
    {
        obj.OnDestroyObject -= InteractiveObjectOnDestroy;
        _interactiveObjects.Remove(obj);
    }

    public void GenerateNextLevel(Transform transform)
    {
        Destroy(_generator);
        Destroy(_rocket);
        LevelGenerator.Scripts.LevelGenerator generator = Instantiate(_levelGenerator.GetComponent<LevelGenerator.Scripts.LevelGenerator>(), transform.position, Quaternion.identity);
        generator.Init();
        _generator = generator.gameObject;
        InitRocket();
    }

    public void InitRocket()
    {
        
        if (_playerSpawn != null)
        {
            int selectedRocketIndex = DataManager.Instance.SelectedRocketIndex;
            GameObject r = GetComponent<RocketsArray>().PlayerRocketPrefabs[selectedRocketIndex];
            
            _rocket = Instantiate(r, _generator.GetComponentInChildren<Transform>().position, _playerSpawn.rotation);
            _rocket.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    public void LoadLevel(int level = 1)
    {
        SceneManager.LoadScene("Level" + level);
    }

    public void ReplayLevel()
    {
        DataManager.Instance.StarBonus += StarsInCurrentLevel;
        LoadLevelBegin();
        UIManager.Instance.BonusCount = DataManager.Instance.StarBonus;
    }

    public void LoadLevelBegin()
    {
        UIManager.Instance.HideLevelComplete();
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadNextScene()
    {
        DataManager.Instance.StarBonus += StarsInCurrentLevel;
        UIManager.Instance.BonusCount = DataManager.Instance.StarBonus;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        ++currentSceneIndex;
        
        if(currentSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            currentSceneIndex = 0;
        }

        UIManager.Instance.HideLevelComplete();
        SceneManager.LoadScene("Level" + currentSceneIndex);
    }

    public void Landed()
    {
        SoundManager.Instance.PlayWinMusic();
        UIManager.Instance.ShowLevelComplete();
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        currentBuildIndex++;
        DataManager.Instance.SetLevelUnlocked("Level" + currentBuildIndex);
        DataManager.Instance.LastUnlockedLevel = currentBuildIndex;
        DeathCount = 0;
    }

    public BonusPopUp CreateBonusPopUp(Vector3 pos, float amount)
    {
        Transform bonusTransform = Instantiate(_textPopUp, pos, Quaternion.identity);
        BonusPopUp bonusPopUp = bonusTransform.GetComponent<BonusPopUp>();
        bonusPopUp.Setup(amount);
        return bonusPopUp;
    }

    public void StarBonusFound()
    {
        UIManager.Instance.BonusCount += 1;
        UIManager.Instance.StarBonus.text = UIManager.Instance.BonusCount.ToString();
        StarsInCurrentLevel += 1;
    }

    public bool IsConnected()
    {
        StartCoroutine(CheckInternetConnection(IsConnected =>
        {
            isConnectedToNetwork = IsConnected;
        }));
        return isConnectedToNetwork;
    }

    private IEnumerator CheckInternetConnection(Action<bool> action)
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();
        if (request.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
