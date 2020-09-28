using TMPro;
using UnityEngine;

public class BonusPopUp : MonoBehaviour
{
    [SerializeField] private float _speed;

    private TextMeshPro _textMesh;
    private Vector3 _movement;
    private Color _textColor;
    private float _disappearTimer;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshPro>();
        _movement = new Vector3(0, _speed, 0);
    }
    public void Setup(float bonusAmount)
    {
        _textMesh.text = "+" + Mathf.CeilToInt(bonusAmount * 100).ToString();
        _textColor = _textMesh.color;
        _disappearTimer = 1f;
    }

    private void Update()
    {
        transform.position += _movement * Time.deltaTime;
        _disappearTimer -= Time.deltaTime;
        if(_disappearTimer < float.Epsilon)
        {
            float disappearSpeed = 3f;
            _textColor.a -= disappearSpeed * Time.deltaTime;
            _textMesh.color = _textColor;
            if(_textColor.a < float.Epsilon)
            {
                Destroy(gameObject);
            }
        }
    }
}
