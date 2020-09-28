using UnityEngine.UI;
using UnityEngine;

public class LevelCompletePanel : MonoBehaviour
{
    [SerializeField] private Button _doubleRewardBtn;

    private void OnEnable()
    {
        _doubleRewardBtn.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        //_doubleRewardBtn.gameObject.SetActive(false);
    }
}
