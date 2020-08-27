using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private string _currentLevelName = string.Empty;
    private int currentSceneIndex;

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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(!scene.name.Equals("Main") && !scene.name.Equals("Shop"))
        {
            UIManager.Instance.ToggleHUD(true);
            UIManager.Instance.ToggleGameOverUI(false);
            UIManager.Instance.ShowLevelCount();
            UIManager.Instance.EnergyProgress.fillAmount = 1f;
            int selectedRocketIndex = PlayerPrefs.GetInt("SelectedRocketIndex", 0);
            // TODO find launching pad position in scene, instantiate rocket object at it.
            GameObject r = GetComponent<RocketsArray>().PlayerRocketPrefabs[selectedRocketIndex];
            GameObject rocket = Instantiate(r, r.transform.localPosition, r.transform.rotation);
        }

        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }

    public void LoadLevel()
    {
        Initiate.Fade("Level1", Color.black, 1f);
    }

    public void LoadLevelBegin()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadNextScene()
    {
        PlayerPrefs.SetInt("StarBonus", UIManager.Instance.BonusCount);
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        ++currentSceneIndex;
        
        if(currentSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            currentSceneIndex = 0;
        }

        Initiate.Fade("Level" + currentSceneIndex, Color.black, 1f);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
