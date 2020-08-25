using UnityEngine;

public class RocketPlayer : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private int _price;
    [SerializeField] private float _energyAmount;
    [SerializeField] private float _fuelBurnSpeed;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;

    public string Name { get => _name; set => _name = value; }
    public int Price { get => _price; set => _price = value; }
    public float EnergyAmount { get => _energyAmount; set => _energyAmount = value; }
    public float FuelBurnSpeed { get => _fuelBurnSpeed; set => _fuelBurnSpeed = value; }
    public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }
    public float RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }
}
