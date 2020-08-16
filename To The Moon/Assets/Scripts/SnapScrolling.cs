using UnityEngine;

public class SnapScrolling : MonoBehaviour
{
    [SerializeField] private GameObject _scrollPan;

    [Range(0, 500)]
    [SerializeField] private int _panOffset;

    [Range(1, 50)]
    [SerializeField] private int _panCount = 3;

    private GameObject[] _instantiatedPanels;
    private Vector3[] _pansPosition;

    void Start()
    {
        _instantiatedPanels = new GameObject[_panCount];
        _pansPosition = new Vector3[_panCount];

        for(int i = 0; i < _panCount; i++)
        {
            _instantiatedPanels[i] = Instantiate(_scrollPan, transform, false);
            if(i == 0)
            {
                continue;
            }
            float x = _instantiatedPanels[i].transform.localPosition.x;
            float y = _instantiatedPanels[i - 1].transform.localPosition.y - _scrollPan.GetComponent<RectTransform>().sizeDelta.y - _panOffset; ;
            _instantiatedPanels[i].transform.localPosition = new Vector3(x, y, 0);

            _pansPosition[i] = -_instantiatedPanels[i].transform.localPosition;
        }
    }

    private void FixedUpdate()
    {
        
    }
}
