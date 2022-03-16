using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private Button _buttonShowAd;

    [SerializeField] private string adID = "Rewarded_Android";

    public static RewardedAds Instance;
    void Awake()
    {
        _buttonShowAd.interactable = false;
        Instance = this;
    }

    void Start()
    {
        DestroyGameObject();
        LoadAd();
    }

    public void LoadAd()
    {
        Debug.Log($"Loading Ad: {adID}");
        Advertisement.Load(adID, this);
    }

    public void ShowAd()
    {
        _buttonShowAd.interactable = false;
        Debug.Log($"Showing Ad: {adID}");
        Advertisement.Show(adID, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad Loaded: {placementId}");

        if (placementId.Equals(adID))
        {
            _buttonShowAd.onClick.AddListener(ShowAd);
            _buttonShowAd.interactable = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(adID) && showCompletionState.Equals(UnityAdsCompletionState.COMPLETED))
            Debug.Log("Unity Rewarded Ad Completed");
        GameManager.Instance.ContinueGame();
    }

    private void OnDestroy()
    {
        _buttonShowAd.onClick.RemoveAllListeners();
    }

    void DestroyGameObject()
    {
        if (PlayerPrefs.HasKey("noAdsBuy"))
        {
            _buttonShowAd.interactable = true;
            Destroy(this);
        }
    }
}
