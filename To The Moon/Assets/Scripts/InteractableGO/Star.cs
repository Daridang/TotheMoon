using UnityEngine;

public class Star : Interactive, ICollectable
{
    [SerializeField] private GameObject _explode;
    [SerializeField] private float rotationSpeedX = 0f;
    [SerializeField] private float rotationSpeedY = 50f;
    [SerializeField] private float rotationSpeedZ = 0f;
    [SerializeField] private FloatRange _floatRange;
    private Vector3 rotationAxis;
    private float _starBonus;

    protected override void Init()
    {
        _explodeParticles = _explode;
        _starBonus = _floatRange.RandomInRange;
        rotationAxis = new Vector3(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, rotationSpeedZ * Time.deltaTime);
    }

    public void CollectableAction()
    {
        gameObject.transform.Rotate(rotationAxis);
    }

    protected override void InteractWithPlayer(GameObject player)
    {
        GameManager.Instance.StarBonusFound();
    }
}