using UnityEngine;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject _hud;

    private void Start()
    {
        
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
        Initiate.Fade("Shop", Color.black, 1f);
        UIManager.Instance.MainMenu(false);
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
