using UnityEngine;

public class CollectableExplosion : MonoBehaviour
{
    [SerializeField] private GameObject _explode;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Instantiate(_explode, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
