using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionPanel : MonoBehaviour
{
    public void OpenMainMenu()
    {
        UIManager.Instance.LoadMainMenuScene();
    }

    public void OpenShopScene()
    {
        SoundManager.Instance.PlayClickedSound();
        SceneManager.LoadScene("Shop");
        UIManager.Instance.ShowShopUI();
    }
}
