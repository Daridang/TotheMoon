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
    [SerializeField] private GameObject _gameOverUI;

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

    public void MainMenu(bool show)
    {
        _mainMenu.gameObject.SetActive(show);
    }

    public void ToggleHUD(bool isEnable)
    {
        _mainMenu.Hud.SetActive(isEnable);
        BonusCount = PlayerPrefs.GetInt("StarBonus", 0);
        _starBonus.text = BonusCount.ToString();
    }

    public void ShowLevelCount()
    {
        LevelText.gameObject.SetActive(true);
        LevelText.text = "Level " + (SceneManager.GetActiveScene().buildIndex).ToString();
    }

    public void ToggleGameOverUI(bool b)
    {
        _gameOverUI.SetActive(b);
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

