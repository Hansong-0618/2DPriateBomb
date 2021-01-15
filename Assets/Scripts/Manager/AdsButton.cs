using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System;

public class AdsButton : MonoBehaviour, IUnityAdsListener
{
#if UNITY_ANDROID
    private string gameID = "3968195";
#elif UNITY_IOS
    private string gameID = "3968194";
#endif
    Button adsButton;
    public string placementID = "rewardedVideo";

    void Start()
    {
        adsButton = GetComponent<Button>();

        if (adsButton)
        {
            adsButton.onClick.AddListener(ShowRewardAds);
        }

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameID, true);
    }

    public void ShowRewardAds()
    {
        Advertisement.Show(placementID);
    }

    public void OnUnityAdsDidError(string message)
    {

    }
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Failed:
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Finished:
                Debug.Log("广告播放完了，发放奖励！");
                FindObjectOfType<PlayerController>().health = 3;
                FindObjectOfType<PlayerController>().isDead = false;
                UIManager.instance.UpdateHealth(FindObjectOfType<PlayerController>().health);
                break;
        }
    }
    public void OnUnityAdsDidStart(string placementId)
    {

    }
    public void OnUnityAdsReady(string placementId)
    {
        if (Advertisement.IsReady(placementId))
        {
            Debug.Log("广告准备好了！");
        }
    }
}
