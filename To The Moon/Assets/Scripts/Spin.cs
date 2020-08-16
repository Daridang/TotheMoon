using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float rotationSpeedX = 0f;
    [SerializeField] private float rotationSpeedY = 0f;
    [SerializeField] private float rotationSpeedZ = 50f;
    private Vector3 rotationAxis;

    private void Start()
    {
        rotationAxis = new Vector3(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, rotationSpeedZ * Time.deltaTime);
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.Rotate(rotationAxis);
    }
}
