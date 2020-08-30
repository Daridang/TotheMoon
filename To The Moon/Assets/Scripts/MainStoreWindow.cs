using UnityEngine;

public class MainStoreWindow : MonoBehaviour
{
    public void ExitStore()
    {
        UIManager.Instance.LoadMainMenuScene();
    }
}
