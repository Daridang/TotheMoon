using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class SwipeManager : MonoBehaviour
{
    #region Inspector Variables

    [Tooltip("Min swipe distance (inches) to register as swipe")]
    [SerializeField] float minSwipeLength = 0.5f;

    [Tooltip("If true, a swipe is counted when the min swipe length is reached. If false, a swipe is counted when the touch/click ends.")]
    [SerializeField] bool triggerSwipeAtMinLength = false;

    [Tooltip("Whether to detect eight or four cardinal directions")]
    [SerializeField] bool useEightDirections = false;

    [Range(5, 50)]
    [SerializeField] private int _distanceBetweenObjects;
    [SerializeField] private TextMeshProUGUI _rocketName;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private Text _speed;
    [SerializeField] private Text _energy;
    [SerializeField] private Text _acceleration;
    [SerializeField] private GameObject _leftArrow;
    [SerializeField] private GameObject _rightArrow;
    [SerializeField] private GameObject _buyButton;
    [SerializeField] private GameObject _selectButton;
    [SerializeField] private GameObject _notEnoughStarsPanel;
    [SerializeField] private TextMeshProUGUI _selectBtnText;
    [SerializeField] private TextMeshProUGUI _starBonusCount;
    [SerializeField] private Text _unlockText;
    [SerializeField] private float _itemMovementSpeed = 3f;

    #endregion

    private GameObject[] _gameObjects;
    private int _currentObject;
    private bool _isSwipeLeft = false;
    private bool _isSwipeRight = false;
    private bool _isSwipeDisabled = false;

    const float eightDirAngle = 0.906f;
    const float fourDirAngle = 0.5f;
    const float defaultDPI = 72f;
    const float dpcmFactor = 2.54f;

    static Dictionary<Swipe, Vector2> cardinalDirections = new Dictionary<Swipe, Vector2>()
    {
        { Swipe.Up,         CardinalDirection.Up                 },
        { Swipe.Down,         CardinalDirection.Down             },
        { Swipe.Right,         CardinalDirection.Right             },
        { Swipe.Left,         CardinalDirection.Left             },
        { Swipe.UpRight,     CardinalDirection.UpRight             },
        { Swipe.UpLeft,     CardinalDirection.UpLeft             },
        { Swipe.DownRight,     CardinalDirection.DownRight         },
        { Swipe.DownLeft,     CardinalDirection.DownLeft         }
    };

    public delegate void OnSwipeDetectedHandler(Swipe swipeDirection, Vector2 swipeVelocity);

    static OnSwipeDetectedHandler _OnSwipeDetected;
    public static event OnSwipeDetectedHandler OnSwipeDetected
    {
        add
        {
            _OnSwipeDetected += value;
            autoDetectSwipes = true;
        }
        remove
        {
            _OnSwipeDetected -= value;
        }
    }

    public static Vector2 swipeVelocity;

    static float dpcm;
    static float swipeStartTime;
    static float swipeEndTime;
    static bool autoDetectSwipes;
    static bool swipeEnded;
    static Swipe swipeDirection;
    static Vector2 firstPressPos;
    static Vector2 secondPressPos;
    static SwipeManager instance;


    void Awake()
    {
        instance = this;
        float dpi = (Screen.dpi == 0) ? defaultDPI : Screen.dpi;
        dpcm = dpi / dpcmFactor;

        _gameObjects = new GameObject[GameManager.Instance.GetComponent<RocketsArray>().RocketPrefabs.Length];
        for(int i = 0; i < GameManager.Instance.GetComponent<RocketsArray>().RocketPrefabs.Length; i++)
        {
            _gameObjects[i] = Instantiate(GameManager.Instance.GetComponent<RocketsArray>().RocketPrefabs[i]);
        }
        _currentObject = 0;
        _gameObjects[_currentObject].transform.position = new Vector3();

        float lastDistance = 0f;
        for(int i = 1; i < _gameObjects.Length; i++)
        {
            lastDistance += _distanceBetweenObjects;
            _gameObjects[i].transform.position = new Vector3(lastDistance, 0, 0);
        }

        _starBonusCount.text = DataManager.Instance.StarBonus.ToString();
    }

    private void Start()
    {
        _rocketName.text = _gameObjects[0].GetComponent<StoreItem>().RocketData.Name;
        _price.text = _gameObjects[0].GetComponent<StoreItem>().RocketData.Price.ToString();
        _speed.text = (_gameObjects[0].GetComponent<StoreItem>().RocketData.MovementSpeed * 10).ToString();
        _energy.text = (100 * _gameObjects[0].GetComponent<StoreItem>().RocketData.FuelBurnSpeed).ToString();
        _acceleration.text = _gameObjects[0].GetComponent<StoreItem>().RocketData.Acceleration.ToString();
        _leftArrow.SetActive(false);
        UpdateShopUI();
    }

    private IEnumerator MoveItem(Vector3 from, Vector3 to, float time, GameObject obj)
    {
        float counter = 0;
        Vector3 start = from;

        while(counter < time)
        {
            counter += Time.deltaTime * _itemMovementSpeed;
            from = Vector3.Lerp(start, to, counter);
            obj.transform.position = from;
            yield return null;
        }
    }

    public void Buy()
    {
        int starBonusCount = DataManager.Instance.StarBonus;
        if(starBonusCount < _gameObjects[_currentObject].GetComponent<StoreItem>().RocketData.Price)
        {
            _notEnoughStarsPanel.SetActive(true);
            _isSwipeDisabled = true;
            _gameObjects[_currentObject].SetActive(false);
        }
        else
        {
            int starsLeft = starBonusCount - _gameObjects[_currentObject].GetComponent<StoreItem>().RocketData.Price;
            DataManager.Instance.StarBonus = starsLeft;
            
            DataManager.Instance.SetUnlocked(_gameObjects[_currentObject].name);
            UpdateShopUI();
        }
    }

    public void CloseWarningPanel()
    {
        _notEnoughStarsPanel.SetActive(false);
        _isSwipeDisabled = false;
        _gameObjects[_currentObject].SetActive(true);
    }

    public void Select()
    {
        foreach(GameObject g in _gameObjects)
        {
            g.GetComponent<StoreItem>().RocketData.IsSelected = false;
            UpdateShopUI();
        }
        _gameObjects[_currentObject].GetComponent<StoreItem>().RocketData.IsSelected = true;
        _selectBtnText.text = "Ready";
        UpdateShopUI();
        DataManager.Instance.SelectedRocketIndex = _currentObject;
    }

    private void Arrows()
    {
        if(_currentObject == 0)
        {
            _leftArrow.SetActive(false);
        }
        else
        {
            _leftArrow.SetActive(true);
        }

        if(_currentObject == _gameObjects.Length - 1)
        {
            _rightArrow.SetActive(false);
        }
        else
        {
            _rightArrow.SetActive(true);
        }
    }

    private void UpdateShopUI()
    {
        if(DataManager.Instance.CheckRocketIsUnlocked(_gameObjects[_currentObject].name) == 1)
        {
            _buyButton.SetActive(false);
            _unlockText.text = "";
            _selectButton.SetActive(true);
            if(_gameObjects[_currentObject].GetComponent<StoreItem>().RocketData.IsSelected)
            {
                _selectBtnText.text = "Ready";
            }
            else
            {
                _selectBtnText.text = "Select";
            }
        }
        else
        {
            _buyButton.SetActive(true);
            _selectButton.SetActive(false);
            _unlockText.text = "Unlock";
            _price.text = _gameObjects[_currentObject].GetComponent<StoreItem>().RocketData.Price.ToString();
        }

        _starBonusCount.text = DataManager.Instance.StarBonus.ToString();
    }

    public void MoveLeft()
    {
        if(!_isSwipeDisabled)
        {
            if(_currentObject < _gameObjects.Length - 1)
            {
                for(int i = 0; i < _gameObjects.Length; i++)
                {
                    float max = _gameObjects[i].transform.position.x - _distanceBetweenObjects;
                    StartCoroutine(MoveItem(_gameObjects[i].transform.position, new Vector3(max, 0, 0), 1f, _gameObjects[i]));
                }
                _isSwipeLeft = false;
                _currentObject++;

                _rocketName.text = _gameObjects[_currentObject].GetComponent<StoreItem>().RocketData.Name;
                _price.text = _gameObjects[_currentObject].GetComponent<StoreItem>().RocketData.Price.ToString();
                _speed.text = (_gameObjects[_currentObject].GetComponent<StoreItem>().RocketData.MovementSpeed * 10).ToString();
                _energy.text = (100 * _gameObjects[_currentObject].GetComponent<StoreItem>().RocketData.FuelBurnSpeed).ToString();
                _acceleration.text = _gameObjects[_currentObject].GetComponent<StoreItem>().RocketData.Acceleration.ToString();
            }
            Arrows();
            UpdateShopUI();
        }
    }

    public void MoveRight()
    {
        if(!_isSwipeDisabled)
        {
            if(_currentObject > 0)
            {
                for(int i = 0; i < _gameObjects.Length; i++)
                {
                    float max = _gameObjects[i].transform.position.x + _distanceBetweenObjects;
                    StartCoroutine(MoveItem(_gameObjects[i].transform.position, new Vector3(max, 0, 0), 1f, _gameObjects[i]));
                }
                _isSwipeRight = false;
                _currentObject--;

                _rocketName.text = _gameObjects[_currentObject].GetComponent<StoreItem>().RocketData.Name;
                _price.text = _gameObjects[_currentObject].GetComponent<StoreItem>().RocketData.Price.ToString();
                _speed.text = (_gameObjects[_currentObject].GetComponent<StoreItem>().RocketData.MovementSpeed * 10).ToString();
                _energy.text = (100 * _gameObjects[_currentObject].GetComponent<StoreItem>().RocketData.FuelBurnSpeed).ToString();
                _acceleration.text = _gameObjects[_currentObject].GetComponent<StoreItem>().RocketData.Acceleration.ToString();
            }
            Arrows();
            UpdateShopUI();
        }
    }

    void Update()
    {
        if(!_isSwipeDisabled)
        {
            if(IsSwipingLeft())
            {
                _isSwipeLeft = true;
            }

            if(IsSwipingRight())
            {
                _isSwipeRight = true;
            }

            if(_currentObject == _gameObjects.Length - 1)
            {
                _isSwipeLeft = false;
            }

            if(_currentObject == 0)
            {
                _isSwipeRight = false;
            }

            if(_isSwipeLeft)
            {
                MoveLeft();
            }

            if(_isSwipeRight)
            {
                MoveRight();
            }
        }
    }

    /// <summary>
    /// Attempts to detect the current swipe direction.
    /// Should be called over multiple frames in an Update-like loop.
    /// </summary>
    void DetectSwipe()
    {
        if(GetTouchInput() || GetMouseInput())
        {
            // Swipe already ended, don't detect until a new swipe has begun
            if(swipeEnded)
            {
                return;
            }

            Vector2 currentSwipe = secondPressPos - firstPressPos;
            float swipeCm = currentSwipe.magnitude / dpcm;

            // Check the swipe is long enough to count as a swipe (not a touch, etc)
            if(swipeCm < instance.minSwipeLength)
            {
                // Swipe was not long enough, abort
                if(!instance.triggerSwipeAtMinLength)
                {
                    if(Application.isEditor)
                    {
                        Debug.Log("[SwipeManager] Swipe was not long enough.");
                    }

                    swipeDirection = Swipe.None;
                }

                return;
            }

            swipeEndTime = Time.time;
            swipeVelocity = currentSwipe * (swipeEndTime - swipeStartTime);
            swipeDirection = GetSwipeDirByTouch(currentSwipe);
            swipeEnded = true;
        }
        else
        {
            swipeDirection = Swipe.None;
        }
    }

    public bool IsSwiping() { return swipeDirection != Swipe.None; }
    public bool IsSwipingRight() { return IsSwipingDirection(Swipe.Right); }
    public bool IsSwipingLeft() { return IsSwipingDirection(Swipe.Left); }
    public bool IsSwipingUp() { return IsSwipingDirection(Swipe.Up); }
    public bool IsSwipingDown() { return IsSwipingDirection(Swipe.Down); }
    public bool IsSwipingDownLeft() { return IsSwipingDirection(Swipe.DownLeft); }
    public bool IsSwipingDownRight() { return IsSwipingDirection(Swipe.DownRight); }
    public bool IsSwipingUpLeft() { return IsSwipingDirection(Swipe.UpLeft); }
    public bool IsSwipingUpRight() { return IsSwipingDirection(Swipe.UpRight); }

    #region Helper Functions

    bool GetTouchInput()
    {
        if(Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);

            // Swipe/Touch started
            if(t.phase == TouchPhase.Began)
            {
                firstPressPos = t.position;
                swipeStartTime = Time.time;
                swipeEnded = false;
                // Swipe/Touch ended
            }
            else if(t.phase == TouchPhase.Ended)
            {
                secondPressPos = t.position;
                return true;
                // Still swiping/touching
            }
            else
            {
                // Could count as a swipe if length is long enough
                if(instance.triggerSwipeAtMinLength)
                {
                    return true;
                }
            }
        }

        return false;
    }

    bool GetMouseInput()
    {
        // Swipe/Click started
        if(Input.GetMouseButtonDown(0))
        {
            firstPressPos = (Vector2)Input.mousePosition;
            swipeStartTime = Time.time;
            swipeEnded = false;
            // Swipe/Click ended
        }
        else if(Input.GetMouseButtonUp(0))
        {
            secondPressPos = (Vector2)Input.mousePosition;
            return true;
            // Still swiping/clicking
        }
        else
        {
            // Could count as a swipe if length is long enough
            if(instance.triggerSwipeAtMinLength)
            {
                return true;
            }
        }

        return false;
    }

    static bool IsDirection(Vector2 direction, Vector2 cardinalDirection)
    {
        var angle = instance.useEightDirections ? eightDirAngle : fourDirAngle;
        return Vector2.Dot(direction, cardinalDirection) > angle;
    }

    static Swipe GetSwipeDirByTouch(Vector2 currentSwipe)
    {
        currentSwipe.Normalize();
        var swipeDir = cardinalDirections.FirstOrDefault(dir => IsDirection(currentSwipe, dir.Value));
        return swipeDir.Key;
    }

    bool IsSwipingDirection(Swipe swipeDir)
    {
        DetectSwipe();
        return swipeDirection == swipeDir;
    }

    #endregion
}