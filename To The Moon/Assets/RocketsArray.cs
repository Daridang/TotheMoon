using UnityEngine;

public class RocketsArray : MonoBehaviour
{
    [SerializeField] private GameObject[] _rocketPrefabs;

    public GameObject[] RocketPrefabs { get => _rocketPrefabs; set => _rocketPrefabs = value; }
}
