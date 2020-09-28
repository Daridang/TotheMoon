using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10f;

    void FixedUpdate()
    {
        gameObject.transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
