using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PopulateButtons : MonoBehaviour
{
    [SerializeField] private GameObject _btnPrefab;
    [SerializeField] private Sprite _unlockedLevel;
    [SerializeField] private Sprite _lockedLevel;

    void Start()
    {
        int lvlNr = 1;
        for(int i = 0; i < 10; i++)
        {
            GameObject go = Instantiate(_btnPrefab, gameObject.transform);
            Image[] imgs = go.GetComponentsInChildren<Image>();

            if(DataManager.Instance.CheckLevelIsUnlocked("Level" + lvlNr) == 1)
            {
                go.GetComponent<Image>().sprite = _unlockedLevel;
                imgs[1].gameObject.SetActive(false);
            }
            else
            {
                go.GetComponent<Image>().sprite = _lockedLevel;
                imgs[1].gameObject.SetActive(true);
            }

            go.GetComponentInChildren<TextMeshProUGUI>().text = lvlNr++.ToString();
        }
    }
}
