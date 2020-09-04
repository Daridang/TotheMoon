using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine;

public class AdsListener : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] private Button _showAdBtn;

    private string myPlacementId = "rewardedVideo";

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch(placementId)
        {
            case "rewardedVideo":
                if(showResult == ShowResult.Finished)
                {
                    UIManager.Instance.UpdateUI();
                    _showAdBtn.gameObject.SetActive(false);
                }
                //else if(showResult == ShowResult.Skipped)
                //{
                //    // Do not reward the user for skipping the ad.
                //}
                //else if(showResult == ShowResult.Failed)
                //{
                //    Debug.LogWarning("The ad did not finish due to an error.");
                //}
                break;
            case "ShieldReward":
                UIManager.Instance.ShieldProgress.fillAmount = 1f;
                GameManager.Instance.LoadLevelBegin();
                GameManager.Instance.DeathCount = 0;
                break;
            default:
                break;
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if(placementId == myPlacementId)
        {
            // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

}
