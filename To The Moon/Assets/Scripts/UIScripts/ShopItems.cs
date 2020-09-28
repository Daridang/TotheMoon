using UnityEngine;

public class ShopItems : MonoBehaviour
{
    [SerializeField] private GameObject[] _items;
    private int _index = 0;
    private int _activeIndex = 0;

    void Start()
    {
        foreach(GameObject go in _items)
        {
            go.SetActive(false);
        }
        _activeIndex = _index;
        _items[_activeIndex].SetActive(true);
    }

    public void NextItem()
    {
        _index++;
        if(_index > _items.Length - 1)
        {
            _index = 0;
        }
        _items[_activeIndex].SetActive(false);
        _items[_index].SetActive(true);
        _activeIndex = _index;
    }

    public void PreviousItem()
    {
        _index--;
        if(_index < 0)
        {
            _index = _items.Length - 1;
        }
        _items[_activeIndex].SetActive(false);
        _items[_index].SetActive(true);
        _activeIndex = _index;
    }
}
