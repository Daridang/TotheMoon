using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementControllerTest : MonoBehaviour
{

    private SimpleTouchController _leftController;
    private SimpleTouchController _rightController;
    private float _movementSpeed;
    private float _rotationSpeed;

	private Rigidbody _rigidBody;
    private RocketPlayer _rocket;

    void Awake()
	{
        _leftController = GameObject.FindGameObjectWithTag("LeftTouch").GetComponent<SimpleTouchController>();
        _rightController = GameObject.FindGameObjectWithTag("RightTouch").GetComponent<SimpleTouchController>();
        _rigidBody = GetComponent<Rigidbody>();
        _rocket = GetComponent<RocketPlayer>();
        _movementSpeed = _rocket.MovementSpeed;
        _rotationSpeed = _rocket.RotationSpeed;
    }

    void Update()
	{
        if(_leftController.TouchPresent)
        {
            _rigidBody.AddRelativeForce(Vector3.up * _leftController.GetTouchPosition.y *  _movementSpeed);
        }
        Quaternion rot = Quaternion.Euler(0f, 0f,
                transform.localEulerAngles.z + _rightController.GetTouchPosition.x * -_rotationSpeed);

        _rigidBody.MoveRotation(rot);
    }
}
