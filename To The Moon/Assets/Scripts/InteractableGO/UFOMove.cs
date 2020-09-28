using UnityEngine;

public class UFOMove : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] ParticleSystem _puff;

    private void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Rocket>().ReactOnObstacle();
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
            ParticleSystem ps = Instantiate(_puff, gameObject.transform);
            ps.Play();
            Destroy(gameObject, 0.5f);
        }
    }
}
