using UnityEngine;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject _hud;

    public GameObject Hud { get => _hud; set => _hud = value; }
    
    public void Play()
    {
        SoundManager.Instance.PlayClickedSound();
        GameManager.Instance.LoadLevel();
        UIManager.Instance.MainMenu(false);
    }

    public void OpenShopScene()
    {
        SoundManager.Instance.PlayClickedSound();
        Initiate.Fade("Shop", Color.black, 1f);
        UIManager.Instance.ShowShopUI();
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
