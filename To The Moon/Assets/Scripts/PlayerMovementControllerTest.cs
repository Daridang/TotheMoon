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
            //Thursting(true);
        }
        else
        {
            //Thursting(false);
        }
        //_rigidbody.MovePosition(
        //          transform.position + 
        //          (transform.up * _leftController.GetTouchPosition.y * Time.deltaTime * _movementSpeed));

        

        Quaternion rot = Quaternion.Euler(0f, 0f,
                transform.localEulerAngles.z + _rightController.GetTouchPosition.x * -_rotationSpeed);

        _rigidBody.MoveRotation(rot);
    }

    public void Thursting(bool isPressed)
    {
        //if(isPressed && state == State.Alive)
        //{
        //    _rigidBody.AddRelativeForce(Vector3.up * _mainThrust * Time.deltaTime);
        //    _energy.fillAmount -= _fuelBurnSpeed * Time.deltaTime;
        //    engine.Play();
        //}
        //else if(state != State.Alive)
        //{
        //    engine.Stop();
        //}
        //else
        //{
        //    _audioSource.Stop();
        //    engine.Stop();
        //}

        //if(_energy.fillAmount < float.Epsilon)
        //{
        //    state = State.Dying;
        //    _audioSource.Stop();
        //    gameOverUI.SetActive(true);

        //}
    }

    public void RotateLeft(bool isPressed)
    {
        //if(state == State.Alive && isPressed)
        //{
        //    _rigidBody.freezeRotation = true;

        //    float rotationThisFrame = _rcsThrust * Time.deltaTime;

        //    _rigidBody.transform.Rotate(Vector3.forward * rotationThisFrame);

        //    _rigidBody.freezeRotation = false;
        //}
    }

    public void RotateRight(bool isPressed)
    {
        //if(state == State.Alive && isPressed)
        //{
        //    _rigidBody.freezeRotation = true;

        //    float rotationThisFrame = _rcsThrust * Time.deltaTime;

        //    _rigidBody.transform.Rotate(Vector3.back * rotationThisFrame);

        //    _rigidBody.freezeRotation = false;
        //}
    }
}
