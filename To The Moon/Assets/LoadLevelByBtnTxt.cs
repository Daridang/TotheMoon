using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelByBtnTxt : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _btnText;

    public void PlayLevel()
    {
        if(DataManager.Instance.CheckLevelIsUnlocked("Level" + _btnText.text) == 1)
        {
            SceneManager.LoadScene("Level" + _btnText.text);
        }
    }
}
