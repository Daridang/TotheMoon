using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private string _currentLevelName = string.Empty;
    private int currentSceneIndex;

    public int StarsInCurrentLevel { get; set; }

    public string GetLevelName()
    {
        return SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void GameOver()
    {
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
        if(!scene.name.Equals("Main") && !scene.name.Equals("Shop") && !scene.name.Equals("SelectLevel"))
        {
            StarsInCurrentLevel = 0;
            UIManager.Instance.ToggleHUD(true);
            UIManager.Instance.HideGameOver();
            UIManager.Instance.ShowLevelCount();
            UIManager.Instance.EnergyProgress.fillAmount = 1f;
            int selectedRocketIndex = DataManager.Instance.SelectedRocketIndex;
            GameObject r = GetComponent<RocketsArray>().PlayerRocketPrefabs[selectedRocketIndex];
            r.transform.localPosition = new Vector3(0f, 1f, 0);
            GameObject rocket = Instantiate(r, r.transform.localPosition, r.transform.rotation);
        }
    }

    public void LoadLevel(int level = 1)
    {
        SceneManager.LoadScene("Level" + level);
    }

    public void LoadLevelBegin()
    {
        UIManager.Instance.HideLevelComplete();
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadNextScene()
    {
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

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
