using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private ParticleSystem _puff;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ParticleSystem ps = Instantiate(_puff, other.gameObject.transform);
            ps.Play();
        }
    }
}
