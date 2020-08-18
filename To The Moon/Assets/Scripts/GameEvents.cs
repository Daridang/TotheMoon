using System;

public class GameEvents : Singleton<GameEvents>
{
    public event Action onCollectableFound;

    public void CollectableFound()
    {
        onCollectableFound?.Invoke();
    }
}
