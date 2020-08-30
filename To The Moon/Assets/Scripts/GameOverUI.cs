using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {
    public void Quit() {
        Application.Quit();
    }

    public void Retry() {
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
