using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject _hud;
    [SerializeField] private Button _endlessRunBtnImage;

    public GameObject Hud { get => _hud; set => _hud = value; }

    private void Awake()
    {
        if(DataManager.Instance.CheckLevelIsUnlocked("Level5") == 1)
        {
            _endlessRunBtnImage.enabled = true;
        }
        else
        {
            _endlessRunBtnImage.enabled = false;
        }
    }

    public void Play()
    {
        SoundManager.Instance.PlayClickedSound();
        GameManager.Instance.LoadLevel();
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
}
