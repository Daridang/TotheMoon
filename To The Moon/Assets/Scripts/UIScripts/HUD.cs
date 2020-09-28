using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;

    void Start()
    {
        //_levelText.gameObject.SetActive(true);
        //_levelText.text = "Level " + (SceneManager.GetActiveScene().buildIndex).ToString();
    }
}
