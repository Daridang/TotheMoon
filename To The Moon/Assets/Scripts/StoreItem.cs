using UnityEngine;

public class StoreItem : MonoBehaviour, IStoreItem
{
    public enum RocketType
    {
        TYPE_1,
        TYPE_2,
        TYPE_3
    }

    [SerializeField] private RocketType _rocketType;
    [SerializeField] private int _price;
    [SerializeField] private string _name;

    public string Name { get => _name; set => _name = value; }
    public int Price { get => _price; set => _price = value; }
    public RocketType RocketType1 { get => _rocketType; set => _rocketType = value; }
}
