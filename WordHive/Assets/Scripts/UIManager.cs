using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject gamePanel;
    [SerializeField] private TMP_Text remainingMovesText;

    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TMP_Text endGamePanelTitle;
    [SerializeField] private TMP_Text endGamePanelStats;
    private int totalGames;
    private int wonGames;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
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

    public void UpdateRemainingMovesText(int remainingMoves)
    {
        remainingMovesText.text = remainingMoves.ToString() + " moves left";
    }

    public void ShowEndGamePanel(bool won)
    {
        endGamePanelTitle.text = won ? "Victory" : "Defeat";
        wonGames = PlayerPrefs.GetInt("wonGames");
        totalGames = PlayerPrefs.GetInt("totalGames");
        endGamePanelStats.text = wonGames + " / " + totalGames;
        endGamePanel.SetActive(true);
    }

    public void HideEndGamePanel()
    {
        endGamePanel.SetActive(false);
    }

}
