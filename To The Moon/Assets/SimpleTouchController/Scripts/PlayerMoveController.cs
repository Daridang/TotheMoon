using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveController : MonoBehaviour
{

    [SerializeField] private SimpleTouchController _leftController;
    [SerializeField] private SimpleTouchController _rightController;
    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private float _rotationSpeed = 1f;

	private Rigidbody _rigidbody;
    private Rocket _rocket;

    void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
        _rocket = GetComponent<Rocket>();
    }

    void Update()
	{
        if(_leftController.TouchPresent || _rightController.TouchPresent)
        {
            _rocket.Thursting(true);
        }
        else
        {
            _rocket.Thursting(false);
        }
		_rigidbody.MovePosition(
            transform.position + 
            (transform.up * _leftController.GetTouchPosition.y * Time.deltaTime * _movementSpeed));

        Quaternion rot = Quaternion.Euler(0f, 0f,
                transform.localEulerAngles.z + _rightController.GetTouchPosition.x * -_rotationSpeed);

        _rigidbody.MoveRotation(rot);
    }
}
