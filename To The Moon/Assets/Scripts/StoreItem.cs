using UnityEngine;

public class StoreItem : MonoBehaviour, IStoreItem
{
    [SerializeField] private RocketData _rocketData;

    public RocketData RocketData { get => _rocketData; set => _rocketData = value; }
}
