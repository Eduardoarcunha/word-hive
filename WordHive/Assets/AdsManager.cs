using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    [SerializeField] private GameObject adsPanel;

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
    }

    public void HideAdsPanel()
    {
        adsPanel.SetActive(false);
    }
}
