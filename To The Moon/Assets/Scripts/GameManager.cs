using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Transform _playerSpawn;
    [SerializeField] private GameObject _levelGenerator;

    private string _currentLevelName = string.Empty;
    private int currentSceneIndex;
    private GameObject _generator;
    private GameObject _rocket;
    private bool isConnectedToNetwork = false;


    public int StarsInCurrentLevel { get; set; }
    public int DeathCount { get; set; }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    public void GameOver()
    {
        StarsInCurrentLevel = 0;
        DeathCount++;
        UIManager.Instance.HideHUD();
        UIManager.Instance.ShowGameOver();
    }

    public void LoadMainMenu()
    {
        UIManager.Instance.LoadMainMenuScene();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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
        UIManager.Instance.ShowLevelComplete();
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        currentBuildIndex++;
        DataManager.Instance.SetLevelUnlocked("Level" + currentBuildIndex);
        DataManager.Instance.LastUnlockedLevel = currentBuildIndex;
        DeathCount = 0;
    }

    public void StarBonusFound()
    {
        UIManager.Instance.BonusCount += 1;
        UIManager.Instance.StarBonus.text = UIManager.Instance.BonusCount.ToString();
        GameManager.Instance.StarsInCurrentLevel += 1;
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
