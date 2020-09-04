using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private Button _endlessRunBtnImage;
    [SerializeField] private Button _infoButton;
    [SerializeField] private Sprite _endlessRunEnabled;
    [SerializeField] private Sprite _endlessRunDisabled;

    public GameObject Hud { get => _hud; set => _hud = value; }

    private void Awake()
    {
        EndlessRunBtnActivation();
    }

    public void EndlessRunBtnActivation()
    {
        if(PlayerPrefs.HasKey("Level5"))
        {
            _endlessRunBtnImage.GetComponent<Image>().sprite = _endlessRunEnabled;
            _endlessRunBtnImage.enabled = true;
            _infoButton.gameObject.SetActive(false);
        }
        //if(DataManager.Instance.CheckLevelIsUnlocked("Level5") == 1)
        //{
        //    _endlessRunBtnImage.GetComponent<Image>().sprite = _endlessRunEnabled;
        //    _endlessRunBtnImage.enabled = true;
        //    _infoButton.gameObject.SetActive(false);
        //}
        else
        {
            _endlessRunBtnImage.GetComponent<Image>().sprite = _endlessRunDisabled;
            _endlessRunBtnImage.enabled = false;
            _infoButton.gameObject.SetActive(true);
        }
    }

    public void Play()
    {
        SoundManager.Instance.PlayClickedSound();
        GameManager.Instance.LoadLevel(DataManager.Instance.LastUnlockedLevel);
        UIManager.Instance.MainMenu(false);
    }

    public void OpenShopScene()
    {
        SoundManager.Instance.PlayClickedSound();
        SceneManager.LoadScene("Shop");
        UIManager.Instance.ShowShopUI();
    }

    public void OpenSelectLevelScene()
    {
        SoundManager.Instance.PlayClickedSound();
        SceneManager.LoadScene("SelectLevel");
        gameObject.SetActive(false);
    }

    public void OpenEndlessRunScene()
    {
        //SoundManager.Instance.PlayClickedSound();
        //SceneManager.LoadScene("EndlessRun");
        //gameObject.SetActive(false);
        Debug.Log("TODO Endless run gameplay");
    }

    public void Settings()
    {
        SoundManager.Instance.PlayClickedSound();
    }

    public void ShareOnFacebook()
    {
        // TODO ShareOnFacebook
        SoundManager.Instance.PlayClickedSound();
        //UIManager.Instance.MainMenu(false);
    }

    public void ShowInfoPanel()
    {
        _infoPanel.SetActive(true);
    }

    public void HideInfoPanel()
    {
        _infoPanel.SetActive(false);
    }
}
