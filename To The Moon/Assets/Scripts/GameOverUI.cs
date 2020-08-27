using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {
    public void Quit() {
        Application.Quit();
    }

    public void Retry() {
        Initiate.Fade(SceneManager.GetActiveScene().name, Color.black, 1f);
    }
}
