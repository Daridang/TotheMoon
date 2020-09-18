using System;
using UnityEngine;

public abstract class Interactive : MonoBehaviour
{
    public event Action<Interactive> OnDestroyObject;

    protected GameObject _explodeParticles;

    void Start()
    {
        Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(_explodeParticles, gameObject.transform.position, Quaternion.identity);
            InteractWithPlayer();
            OnDestroyObject?.Invoke(this);
            Destroy(gameObject);
        }
    }

    protected abstract void Init();

    protected abstract void InteractWithPlayer();

}
