using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("WTF: " + other.gameObject.name);
            GameObject go = Instantiate(_explosionPrefab, gameObject.transform);
            Debug.Log("WTF2: " + go.gameObject.name);
            Destroy(gameObject);
        }
    }
}
