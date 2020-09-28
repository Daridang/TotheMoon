using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PopulateButtons : MonoBehaviour
{
    [SerializeField] private GameObject _btnPrefab;
    [SerializeField] private Sprite _unlockedLevel;
    [SerializeField] private Sprite _lockedLevel;
    [SerializeField] private Button _leftBtn;
    [SerializeField] private Button _rightBtn;

    private List<GameObject> gos;
    private Image[] imgs;

    private int _currentPage = 1;
    private int _fullPageSize = 10;
    private int _partPageSize = 0;
    private int _lvlNr = 1;
    private int _tempLvlNr;
    private int _pageCount;
    private int _levels;

    private void Awake()
    {
        _leftBtn.gameObject.SetActive(false);
        gos = new List<GameObject>();
        _levels = 67;//GameManager.Instance.GetLevelCount();
        _pageCount = _levels / _fullPageSize;

        if(_levels % _fullPageSize != 0)
        {
            _partPageSize = _levels % _fullPageSize;
            _pageCount++;
        }

        if(_pageCount == 0)
        {
            _pageCount = 1;
            _rightBtn.gameObject.SetActive(false);
            _leftBtn.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        PopulateFirstPage();
    }

    private void PopulateFirstPage()
    {
        if(_levels < _fullPageSize)
        {
            _fullPageSize = _levels;
        }

        for (int i = 0; i < _fullPageSize; i++)
        {
            GameObject go = Instantiate(_btnPrefab, gameObject.transform);
            gos.Add(go);
            imgs = go.GetComponentsInChildren<Image>();

            if (DataManager.Instance.CheckLevelIsUnlocked("Level" + _lvlNr) == 1)
            {
                go.GetComponent<Image>().sprite = _unlockedLevel;
                imgs[1].gameObject.SetActive(false);
            }
            else
            {
                go.GetComponent<Image>().sprite = _lockedLevel;
                imgs[1].gameObject.SetActive(true);
            }

            go.GetComponentInChildren<TextMeshProUGUI>().text = _lvlNr++.ToString();
        }
    }

    public void MoveLeft()
    {
        _currentPage--;

        if (_currentPage <= 1)
        {
            _currentPage = 1;
            _leftBtn.gameObject.SetActive(false);
        }

        if(_currentPage < _pageCount)
        {
            _rightBtn.gameObject.SetActive(true);
        }

        _tempLvlNr -= _fullPageSize;
        if(_tempLvlNr > 0)
        {
            _lvlNr = _tempLvlNr;
        }
        else
        {
            _lvlNr = 1;
        }

        foreach (GameObject go in gos)
        {
            Destroy(go);
        }
        gos.Clear();

        for (int i = 0; i < _fullPageSize; i++)
        {
            GameObject go = Instantiate(_btnPrefab, gameObject.transform);
            gos.Add(go);

            imgs = go.GetComponentsInChildren<Image>();

            if (DataManager.Instance.CheckLevelIsUnlocked("Level" + _lvlNr) == 1)
            {
                go.GetComponent<Image>().sprite = _unlockedLevel;
                imgs[1].gameObject.SetActive(false);
            }
            else
            {
                go.GetComponent<Image>().sprite = _lockedLevel;
                imgs[1].gameObject.SetActive(true);
            }

            go.GetComponentInChildren<TextMeshProUGUI>().text = _lvlNr++.ToString();
        }
    }

    public void MoveRight()
    {
        int tempPageSize = _fullPageSize;
        _currentPage++;
        if (_currentPage >= _pageCount)
        {
            _currentPage = _pageCount;

            if (_partPageSize != 0)
            {
                tempPageSize = _partPageSize;
            }

            _rightBtn.gameObject.SetActive(false);
        }

        if (_currentPage > 1)
        {
            _leftBtn.gameObject.SetActive(true);
        }

        foreach(GameObject go in gos)
        {
            Destroy(go);
        }
        gos.Clear();

        for (int i = 0; i < tempPageSize; i++)
        {
            GameObject go = Instantiate(_btnPrefab, gameObject.transform);
            gos.Add(go);
            imgs = go.GetComponentsInChildren<Image>();

            if (DataManager.Instance.CheckLevelIsUnlocked("Level" + _lvlNr) == 1)
            {
                go.GetComponent<Image>().sprite = _unlockedLevel;
                imgs[1].gameObject.SetActive(false);
            }
            else
            {
                go.GetComponent<Image>().sprite = _lockedLevel;
                imgs[1].gameObject.SetActive(true);
            }

            go.GetComponentInChildren<TextMeshProUGUI>().text = _lvlNr++.ToString();
            _tempLvlNr = _lvlNr - tempPageSize;
        }
    }
}
