using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string _gameID = "4656593";
    [SerializeField] private bool _testMode = true;
    public static AdsInitializer Instance;

    void Awake()
    {
        DestroyGameObject();
        Advertisement.Initialize(_gameID, _testMode, this);
        Instance = this;
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Initialisation Complete");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Initialisation Failed");
    }

    void DestroyGameObject()
    {
        if (PlayerPrefs.HasKey("noAdsBuy"))
            Destroy(this);
    }
}
