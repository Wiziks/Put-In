using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAdByTime : MonoBehaviour
{
    [SerializeField] private InterstitialAds _interstitialAds;
    [SerializeField] private int _timePeriodToShow = 300;
    float timer;
    bool firstTime;
    bool canShow = true;
    void Update()
    {
        if (!firstTime) return;

        timer += Time.deltaTime;
        if (timer > _timePeriodToShow)
            canShow = true;
    }

    public void ShowAd()
    {
        if (canShow || !firstTime)
        {
            firstTime = true;
            canShow = false;
            timer = 0;
            if (!TutorialScript.Instance)
                _interstitialAds.ShowAd();
        }
    }
}
