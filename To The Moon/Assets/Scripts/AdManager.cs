using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine;

public class AdManager : Singleton<AdManager>
{
    string gameId = "3797631";
    string _myPlacementId = "rewardedVideo";
    string _shieldReward = "ShieldReward";
    bool testMode = true;

    [SerializeField] private AdsListener _listener;

    // Initialize the Ads listener and service:
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Advertisement.AddListener(_listener);
        Advertisement.Initialize(gameId, testMode);
    }

    public void ShowAdForShieldReward()
    {
        if(GameManager.Instance.IsConnected())
        {
            if(Advertisement.IsReady(_shieldReward))
            {
                Advertisement.Show(_shieldReward);
            }
            else
            {
                UIManager.Instance.SetVisibilityForVideoMessage(true);
                Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
            }
        }
        else
        {
            UIManager.Instance.SetVisibilityForNetworkMessage(true);
        }
    }

    public void ShowRewardedVideo()
    {
        // Check if UnityAds ready before calling Show method:
        if(Advertisement.IsReady(_myPlacementId))
        {
            Advertisement.Show(_myPlacementId);
        }
        else
        {
            UIManager.Instance.SetVisibilityForVideoMessage(true);
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    new public void OnDestroy()
    {
        Advertisement.RemoveListener(_listener);
    }
}
