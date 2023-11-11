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
    [SerializeField] private TMP_Text endGameTotalGamesText;
    [SerializeField] private TMP_Text endGameWinPercentText;
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
        endGameTotalGamesText.text = totalGames.ToString();
        endGameWinPercentText.text = (totalGames == 0) ? "0%" : ((int)((float)wonGames / (float)totalGames * 100)).ToString() + "%";
        endGamePanel.SetActive(true);
    }

    public void HideEndGamePanel()
    {
        endGamePanel.SetActive(false);
    }

}
