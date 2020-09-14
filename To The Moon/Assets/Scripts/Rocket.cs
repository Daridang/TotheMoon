using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Rocket : MonoBehaviour
{
    enum State
    {
        Alive, Dying, Transcending
    }

    State state = State.Alive;
    private bool _isSaveToLand = false;

    private Rigidbody _rigidBody;
    private SimpleTouchController _leftController;
    private SimpleTouchController _rightController;
    private float _acceleration;

    [SerializeField] private RocketData _rocketData;
    [SerializeField] private float _invokeTime = 2f;
    [SerializeField] private AudioClip _mainEngine;
    [SerializeField] private AudioClip _deathExplosion;
    [SerializeField] private AudioClip _landing;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private ParticleSystem _engine;
    [SerializeField] private ParticleSystem _explode;
    [SerializeField] private ParticleSystem _teleportation;

    [SerializeField] private bool IsGrounded { get; set; } = false;
    public RocketData RocketData { get => _rocketData; set => _rocketData = value; }

    private void OnEnable()
    {
        GameEvents.Instance.onCollectableFound += OnCollectableFound;
    }

    private void OnDisable()
    {
        GameEvents.Instance.onCollectableFound -= OnCollectableFound;
    }

    private void OnCollectableFound()
    {
        SoundManager.Instance.PlayCollectingSound(gameObject.transform.position);
        UIManager.Instance.EnergyProgress.fillAmount += 1f;
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _leftController = GameObject.FindGameObjectWithTag("LeftTouch").GetComponent<SimpleTouchController>();
        _rightController = GameObject.FindGameObjectWithTag("RightTouch").GetComponent<SimpleTouchController>();
        FindObjectOfType<Camera>().GetComponent<Follow>().target = gameObject.transform;
    }

    void Update()
    {
        if(state == State.Alive)
        {
            ProcessInput();
            RaycastHit hit;
            Debug.DrawRay(
                transform.position,
                transform.TransformDirection(Vector3.down) * 0.7f,
                Color.yellow);

            // Does the ray intersect any objects excluding the player layer
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.2f))
            {
                Debug.Log("Did Hit");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(state != State.Alive) { return; }

        switch(other.gameObject.tag)
        {
            //TODO add Particle Effect for collectables

            case "Fuel":
                GameEvents.Instance.CollectableFound();
                Destroy(other.gameObject);
                break;
            case "StarBonus":
                SoundManager.Instance.PlayCollectingSound(other.gameObject.transform.position);
                GameManager.Instance.StarBonusFound();
                Destroy(other.gameObject);
                break;
            case "Shield":
                SoundManager.Instance.PlayCollectingSound(other.gameObject.transform.position);
                Destroy(other.gameObject);
                UIManager.Instance.ShieldProgress.fillAmount += 1f;
                break;
            case "Teleport":
                GameManager.Instance.GenerateNextLevel(gameObject.transform);
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive) { return; }

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                IsGrounded = true;
                break;
            case "Finish":
                Landing();
                break;
            case "Obstacle":
                ReactOnObstacle();
                break;
            default:
                break;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(!collision.collider.CompareTag("Friendly") && !collision.collider.CompareTag("Finish"))
        {
            if (UIManager.Instance.ShieldProgress.fillAmount <= float.Epsilon)
            {
                state = State.Dying;
                _audioSource.Stop();
                _audioSource.PlayOneShot(_deathExplosion);
                _explode.Play();
                GameManager.Instance.GameOver();
            }
            else
            {
                UIManager.Instance.ShieldProgress.fillAmount -= 0.02f;
            }
        }
    }

    private void Landing()
    {
        state = State.Transcending;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_landing);
        GameManager.Instance.Landed();
    }

    public void ReactOnObstacle()
    {
        if(UIManager.Instance.ShieldProgress.fillAmount <= float.Epsilon)
        {
            state = State.Dying;
            _audioSource.Stop();
            _audioSource.PlayOneShot(_deathExplosion);
            _explode.Play();
            GameManager.Instance.GameOver();
        }
        else
        {
            UIManager.Instance.ShieldProgress.fillAmount -= 0.1f;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(state != State.Alive) { return; }

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                IsGrounded = false;
                break;
            default:
                break;
        }
    }

    private void ProcessInput()
    {
        if(_leftController.TouchPresent)
        {
            _acceleration = _leftController.GetTouchPosition.y * RocketData.Acceleration;
            _rigidBody.AddRelativeForce(Vector3.up * _leftController.GetTouchPosition.y * RocketData.MovementSpeed);
            if(!_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(_mainEngine);
                _engine.Play();
            }
            if(UIManager.Instance.EnergyProgress.fillAmount <= float.Epsilon)
            {
                state = State.Dying;
                _audioSource.Stop();
                GameManager.Instance.GameOver();
            }
            else
            {
                UIManager.Instance.EnergyProgress.fillAmount -= RocketData.FuelBurnSpeed * Time.deltaTime;
            }
        }
        else
        {
            if(_acceleration > 0)
            {
                _rigidBody.AddRelativeForce(Vector3.up * _acceleration);
                _acceleration -= Time.deltaTime;
            }
            else
            {
                _rigidBody.AddRelativeForce(Vector3.up * _acceleration);
                _acceleration += Time.deltaTime;
            }
            _audioSource.Stop();
            _engine.Stop();
        }
        Quaternion rot = Quaternion.Euler(0f, 0f,
                transform.localEulerAngles.z + _rightController.GetTouchPosition.x * -RocketData.RotationSpeed);

        _rigidBody.MoveRotation(rot);
    }

    private void OnDestroy()
    {
        _leftController.EndDrag();
        _rightController.EndDrag();
    }
}
