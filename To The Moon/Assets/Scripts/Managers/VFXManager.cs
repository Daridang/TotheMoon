using UnityEngine;

public class VFXManager : Singleton<VFXManager>
{
    [SerializeField] private ParticleSystem _playerDeath;
    [SerializeField] private ParticleSystem _collectableFound;
    [SerializeField] private ParticleSystem _pinkPuff;
    [SerializeField] private ParticleSystem _whitePuff;
    [SerializeField] private ParticleSystem _skinBrownPuff;
    [SerializeField] private ParticleSystem _greenPuff;
    [SerializeField] private ParticleSystem _ufoPuff;
    [SerializeField] private ParticleSystem _asteroidPuff;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayerDeath(Transform transform)
    {
        ParticleSystem ps = Instantiate(_playerDeath, transform);
        ps.Play();
    }

    public void CollectableFound(Transform transform)
    {
        ParticleSystem ps = Instantiate(_collectableFound, transform);
        ps.Play();
    }

    public void PinkPuff(Transform transform)
    {
        ParticleSystem ps = Instantiate(_pinkPuff, transform);
        ps.Play();
    }

    public void WhitePuff(Transform transform)
    {
        ParticleSystem ps = Instantiate(_whitePuff, transform);
        ps.Play();
    }

    public void SkinBrownPuff(Transform transform)
    {
        ParticleSystem ps = Instantiate(_skinBrownPuff, transform);
        ps.Play();
    }

    public void GreenPuff(Transform transform)
    {
        ParticleSystem ps = Instantiate(_greenPuff, transform);
        ps.Play();
    }

    public void UfoPuff(Transform transform)
    {
        ParticleSystem ps = Instantiate(_ufoPuff, transform);
        ps.Play();
    }

    public void AsteroidPuff(Transform transform)
    {
        ParticleSystem ps = Instantiate(_asteroidPuff, transform);
        ps.Play();
    }
}
