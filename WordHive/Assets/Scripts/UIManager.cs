using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject endGamePanel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            instance = this;
        }
    }

    public void ShowGamePanel()
    {
        gamePanel.SetActive(true);
    }

    public void HideGamePanel()
    {
        gamePanel.SetActive(false);
    }

    public void ShowEndGamePanel()
    {
        endGamePanel.SetActive(true);
    }

    public void HideEndGamePanel()
    {
        endGamePanel.SetActive(false);
    }

}
