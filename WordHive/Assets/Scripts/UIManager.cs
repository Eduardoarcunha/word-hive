using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private TMP_Text remainingMovesText;

    [SerializeField] private GameObject endGameCanvas;
    [SerializeField] private GameObject endGameGrid;
    [SerializeField] private TMP_Text endGameCanvasTitle;
    [SerializeField] private TMP_Text endGameTotalGamesNumber;
    [SerializeField] private TMP_Text endGameWinPercentNumber;
    [SerializeField] private TMP_Text endGameCurrentSequenceNumber;
    [SerializeField] private TMP_Text endGameMaxSequenceNumber;
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

    public void ShowGameCanvas()
    {
        gameCanvas.SetActive(true);
    }

    public void HideGameCanvas()
    {
        gameCanvas.SetActive(false);
    }

    public void UpdateRemainingMovesText(int remainingMoves)
    {
        remainingMovesText.text = remainingMoves.ToString() + " moves left";
    }

    public void ShowEndGameCanvas(bool won, Dictionary<int, char?> answers)
    {
        endGameCanvasTitle.text = won ? "Victory" : "Defeat";
        wonGames = PlayerPrefs.GetInt("wonGames");
        totalGames = PlayerPrefs.GetInt("totalGames");
        endGameTotalGamesNumber.text = totalGames.ToString();
        endGameWinPercentNumber.text = (totalGames == 0) ? "0%" : ((int)((float)wonGames / (float)totalGames * 100)).ToString() + "%";
        endGameCurrentSequenceNumber.text = PlayerPrefs.GetInt("currentSequence").ToString();
        endGameMaxSequenceNumber.text = PlayerPrefs.GetInt("maxSequence").ToString();
        SetAnswerGrid(answers);
        endGameCanvas.SetActive(true);
    }

    public void HideEndGameCanvas()
    {
        endGameCanvas.SetActive(false);
    }

    private void SetAnswerGrid(Dictionary<int, char?> answers)
    {
        for (int i = 0; i < 25; i++)
        {
            if (i / 5 % 2 == 1 && i % 2 == 0) continue;
            endGameGrid.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = answers[i].ToString();
        }
    }

}
