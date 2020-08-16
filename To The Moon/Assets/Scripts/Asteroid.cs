using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10f;

    void FixedUpdate()
    {
        gameObject.transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }
}
