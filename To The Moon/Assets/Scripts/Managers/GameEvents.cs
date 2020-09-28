using System;

public class GameEvents : Singleton<GameEvents>
{
    public event Action onCollectableFound;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CollectableFound()
    {
        onCollectableFound?.Invoke();
    }
}
