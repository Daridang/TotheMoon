using UnityEngine;

public class RocketsArray : MonoBehaviour
{
    [SerializeField] private GameObject[] _rocketPrefabs;
    [SerializeField] private GameObject[] _playerRocketPrefabs;

    public GameObject[] RocketPrefabs { get => _rocketPrefabs; set => _rocketPrefabs = value; }
    public GameObject[] PlayerRocketPrefabs { get => _playerRocketPrefabs; set => _playerRocketPrefabs = value; }
}
