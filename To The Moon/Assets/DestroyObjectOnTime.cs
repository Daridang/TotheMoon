using UnityEngine;

public class DestroyObjectOnTime : MonoBehaviour
{
    [SerializeField] private float _time;

    void Start()
    {
        Destroy(gameObject, _time);
    }
}
