using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Button _left;
    [SerializeField] private Button _right;
    [SerializeField] private Button _boost;

    void Start()
    {
        _levelText.gameObject.SetActive(true);
        _levelText.text = "Level " + (SceneManager.GetActiveScene().buildIndex).ToString();
    }

    public void SetRocket(Rocket rocket)
    {
        //_left.GetComponent<RotateLeft>().SetRocket(rocket);
        //_right.GetComponent<RotateLeft>().SetRocket(rocket);
        //_boost.GetComponent<RotateLeft>().SetRocket(rocket);
        //FindObjectOfType<Camera>().GetComponent<Follow>().target = rocket.gameObject.transform;
    }
}
