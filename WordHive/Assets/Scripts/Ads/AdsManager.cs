using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    [SerializeField] private GameObject adsPanel;
    [SerializeField] RewardedAdsButton rewardedAdsButton;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

    }

    public void ShowAdsPanel()
    {
        adsPanel.SetActive(true);
        rewardedAdsButton.LoadAd();
    }

    public void HideAdsPanel()
    {
        adsPanel.SetActive(false);
    }
}
