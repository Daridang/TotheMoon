using UnityEngine;

public class SpawnedObjectMovement : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] ParticleSystem _puff;

    private void Start()
    {
        Destroy(gameObject, 20f);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Rocket>().ReactOnObstacle();
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            ParticleSystem ps = Instantiate(_puff, gameObject.transform);
            ps.Play();
            Destroy(gameObject, 0.5f);
        }
    }
}
