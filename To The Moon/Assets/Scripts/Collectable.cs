using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private GameObject _fuelObject;
    [SerializeField] private GameObject _particles;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            AudioSource source = GetComponent<AudioSource>();
            source.Play();
           // source.enabled = false;
            Destroy(_fuelObject);
            Destroy(_particles);
            //other.GetComponent<Rocket>().FillEnergy(1f);
        }
    }
}
