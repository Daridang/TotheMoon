using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class Rocket : MonoBehaviour
{
    enum State
    {
        Alive, Dying, Transcending
    }

    State state = State.Alive;
    private int currentSceneIndex;
    private bool _isSaveToLand = false;
    private int _bonusCount = 0;

    private Rigidbody _rigidBody;

    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private Image _energy;
    [SerializeField] private Image _shield;
    [SerializeField] private TextMeshProUGUI _bonus;
    [SerializeField] private float _fSpeed = 0.1f;

    [SerializeField] private float _rcsThrust = 50f;
    [SerializeField] private float _mainThrust = 125f;
    [SerializeField] private float _invokeTime = 2f;
    [SerializeField] private AudioClip _mainEngine;
    [SerializeField] private AudioClip _deathExplosion;
    [SerializeField] private AudioClip _landing;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private ParticleSystem engine;
    [SerializeField] private ParticleSystem explode;


    [SerializeField] private bool IsGrounded { get; set; }

    private void OnEnable()
    {
        Timer.OnTimeEnded += SetStatusDying;
        GameEvents.Instance.onCollectableFound += OnCollectableFound;
    }

    private void OnDisable()
    {
        Timer.OnTimeEnded -= SetStatusDying;
        GameEvents.Instance.onCollectableFound -= OnCollectableFound;
    }

    private void OnCollectableFound()
    {
        SoundManager.Instance.PlayCollectingSound(gameObject.transform.position);
        _energy.fillAmount += 1f;
    }

    private void Awake()
    {
        _bonusCount = PlayerPrefs.GetInt("StarBonus", 0);
        _bonus.text = _bonusCount.ToString();
        _rigidBody = GetComponent<Rigidbody>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
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
                //SoundManager.Instance.PlayCollectingSound(other.gameObject.transform.position);
                Destroy(other.gameObject);
                //_energy.fillAmount += 1f;
                break;
            case "StarBonus":
                SoundManager.Instance.PlayCollectingSound(other.gameObject.transform.position);
                Destroy(other.gameObject);
                _bonusCount += 1;
                _bonus.text = _bonusCount.ToString();
                break;
            case "Shield":
                SoundManager.Instance.PlayCollectingSound(other.gameObject.transform.position);
                Destroy(other.gameObject);
                _shield.fillAmount += 1f;
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
            case "Obstacle":
                ReactOnObstacle();
                break;
            case "Finish":
                Landing();
                break;
            default:
                break;
        }
    }

    private void Landing()
    {
        state = State.Transcending;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_landing);
        Invoke("LoadNextScene", _invokeTime);
    }

    private void ReactOnObstacle()
    {
        if(_shield.fillAmount <= float.Epsilon)
        {
            state = State.Dying;
            _audioSource.Stop();
            _audioSource.PlayOneShot(_deathExplosion);
            explode.Play();
            gameOverUI.SetActive(true);
        }
        else
        {
            _shield.fillAmount -= 0.2f;
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

    private void SetStatusDying()
    {
        state = State.Dying;
        gameOverUI.SetActive(true);
    }

    private void LoadLevelBegin()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void LoadNextScene()
    {
        PlayerPrefs.SetInt("StarBonus", _bonusCount);
        ++currentSceneIndex;
        if(currentSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            currentSceneIndex = 0;
        }
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void ProcessInput()
    {
        Thrusting();
        Rotating();
    }

    private void Rotating()
    {

        _rigidBody.freezeRotation = true;

        float rotationThisFrame = _rcsThrust * Time.deltaTime;

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }

        _rigidBody.freezeRotation = false;
    }

    private void Thrusting()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            _rigidBody.AddRelativeForce(Vector3.up * _mainThrust * Time.deltaTime);
            _energy.fillAmount -= _fSpeed * Time.deltaTime;
            if(!_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(_mainEngine);
            }
            engine.Play();
        }
        else
        {
            _audioSource.Stop();            
            engine.Stop();
        }
    }

    public void Thursting(bool isPressed)
    {
        if(isPressed && state == State.Alive)
        {
            //_rigidBody.AddRelativeForce(Vector3.up * _mainThrust * Time.deltaTime);
            _energy.fillAmount -= _fSpeed * Time.deltaTime;
            if(!_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(_mainEngine);
                engine.Play();
            }
        }
        else if(state != State.Alive)
        {
            engine.Stop();
        }
        else
        {
            _audioSource.Stop();
            engine.Stop();
        }

        if(_energy.fillAmount < float.Epsilon)
        {
            state = State.Dying;
            _audioSource.Stop();
            gameOverUI.SetActive(true);
            
        }
    }

    public void RotateLeft(bool isPressed)
    {
        if(state == State.Alive && isPressed)
        {
            _rigidBody.freezeRotation = true;

            float rotationThisFrame = _rcsThrust * Time.deltaTime;

            _rigidBody.transform.Rotate(Vector3.forward * rotationThisFrame);

            _rigidBody.freezeRotation = false;
        }
    }

    public void RotateRight(bool isPressed)
    {
        if(state == State.Alive && isPressed)
        {
            _rigidBody.freezeRotation = true;

            float rotationThisFrame = _rcsThrust * Time.deltaTime;

            _rigidBody.transform.Rotate(Vector3.back * rotationThisFrame);

            _rigidBody.freezeRotation = false;
        }
    }
}
