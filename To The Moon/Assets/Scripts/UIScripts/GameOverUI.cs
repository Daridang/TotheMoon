using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{

    [SerializeField] private Button _getShieldBtn;

    private void OnEnable()
    {
        if(GameManager.Instance.DeathCount >= 3)
        {
            _getShieldBtn.gameObject.SetActive(true);
        }
        else
        {
            _getShieldBtn.gameObject.SetActive(false);
        }
    }

    public void Quit() {
        Application.Quit();
    }

    public void Retry() {
        SoundManager.Instance.PlayGameMusic();
        Initiate.Fade(SceneManager.GetActiveScene().name, Color.black, 1f);
    }

    public void MainMenu()
    {
        //Initiate.Fade("Main", Color.black, 1f);
        //Debug.Log("WTF: MainMenu() After Initiate");
        //UIManager.Instance.HideGameOver();
        
        GameManager.Instance.LoadMainMenu();
    }
}
