using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private Image _energyProgress;
    [SerializeField] private Image _shieldProgress;
    [SerializeField] private TextMeshProUGUI _starBonus;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _currentLevelStars;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _levelComplete;
    [SerializeField] private GameObject _networkMessage;
    [SerializeField] private GameObject _videoNotReady;

    public Image EnergyProgress { get => _energyProgress; set => _energyProgress = value; }
    public Image ShieldProgress { get => _shieldProgress; set => _shieldProgress = value; }

    public TextMeshProUGUI StarBonus { get => _starBonus; set => _starBonus = value; }
    public TextMeshProUGUI LevelText { get => _levelText; set => _levelText = value; }
    public GameObject GameOverUI { get => _gameOverUI; set => _gameOverUI = value; }
    public int BonusCount { get; set; } = 0;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetVisibilityForNetworkMessage(bool enabled)
    {
        _networkMessage.SetActive(enabled);
    }

    public void SetVisibilityForVideoMessage(bool enabled)
    {
        _videoNotReady.SetActive(enabled);
    }

    public void DoubleReward()
    {
        if (GameManager.Instance.IsConnected())
        {
            AdManager.Instance.ShowRewardedVideo();
        }
        else
        {
            SetVisibilityForNetworkMessage(true);
        }
    }

    public void UpdateUI()
    {
        GameManager.Instance.StarsInCurrentLevel *= 2;
        _currentLevelStars.text = "x " + GameManager.Instance.StarsInCurrentLevel;
    }

    public void ShowLevelComplete()
    {
        _levelComplete.SetActive(true);
        _currentLevelStars.text = "x " + GameManager.Instance.StarsInCurrentLevel;
    }

    public void HideLevelComplete()
    {
        _levelComplete.SetActive(false);
    }

    public void OnLevelComplete()
    {
        _levelComplete.SetActive(true);
    }

    public void LoadMainMenuScene()
    {
        DataManager.Instance.StarBonus += GameManager.Instance.StarsInCurrentLevel;
        _levelComplete.SetActive(false);
        _mainMenu.Hud.SetActive(false);
        _mainMenu.GetComponent<MainMenu>().EndlessRunBtnActivation();
        _gameOverUI.SetActive(false);
        SceneManager.LoadScene("Main");
        _mainMenu.gameObject.SetActive(true);
    }

    public void MainMenu(bool show)
    {
        _mainMenu.gameObject.SetActive(show);
    }

    public void ToggleHUD(bool isEnable)
    {
        _mainMenu.Hud.SetActive(isEnable);
        BonusCount = DataManager.Instance.StarBonus;
        _starBonus.text = BonusCount.ToString();
    }

    public void ShowLevelCount()
    {
        LevelText.gameObject.SetActive(true);
        LevelText.text = "Level " + (SceneManager.GetActiveScene().buildIndex).ToString();
    }

    public void HideGameOver()
    {
        _gameOverUI.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ShowGameOver()
    {
        _gameOverUI.SetActive(true);
    }

    public void HideHUD()
    {
        _mainMenu.Hud.SetActive(false);
    }

    public void ShowShopUI()
    {
        _mainMenu.gameObject.SetActive(false);
    }


}

