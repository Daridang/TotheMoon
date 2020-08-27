using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveController : MonoBehaviour
{

    private SimpleTouchController _leftController;
    private SimpleTouchController _rightController;
    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private float _rotationSpeed = 2f;

	private Rigidbody _rigidbody;
    private Rocket _rocket;

    void Awake()
	{
        _leftController = GameObject.FindGameObjectWithTag("LeftTouch").GetComponent<SimpleTouchController>();
        _rightController = GameObject.FindGameObjectWithTag("RightTouch").GetComponent<SimpleTouchController>();
        _rigidbody = GetComponent<Rigidbody>();
        _rocket = GetComponent<Rocket>();
    }

    void Update()
	{
        if(_leftController.TouchPresent)
        {
            //_rocket.Thursting(true);
        }
        else
        {
            //_rocket.Thursting(false);
        }
		//_rigidbody.MovePosition(
  //          transform.position + 
  //          (transform.up * _leftController.GetTouchPosition.y * Time.deltaTime * _movementSpeed));

        Quaternion rot = Quaternion.Euler(0f, 0f,
                transform.localEulerAngles.z + _rightController.GetTouchPosition.x * -_rotationSpeed);

        _rigidbody.MoveRotation(rot);
    }
}
