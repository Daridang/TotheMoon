using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void MainMenu(bool show)
    {
        _mainMenu.gameObject.SetActive(show);
    }
}

