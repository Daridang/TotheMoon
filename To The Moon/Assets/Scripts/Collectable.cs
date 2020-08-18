using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableType
    {
        FUEL, SHIELD, STAR
    }

    [SerializeField] private CollectableType _type;

    public CollectableType Type { get => _type; set => _type = value; }
}
