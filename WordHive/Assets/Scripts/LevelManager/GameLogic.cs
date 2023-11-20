using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System;

public class GameLogic : MonoBehaviour
{
    private int remainingMoves;
    public bool isMoving = false;
    private const int TOTAL_MOVES = 15;
    private GridManagement gridManagement;

    private Color greenColor = new Color32(24, 111, 101, 255);  // #186f65
    private Color yellowColor = new Color32(255, 193, 2, 255);  // #FFC102
    private Color whiteColor = new Color32(190, 191, 173, 255); // #d2e0fb

    void Start()
    {
        remainingMoves = TOTAL_MOVES;
        gridManagement = GetComponent<GridManagement>();
    }

    public void MoveMade(string[] answerWords, Dictionary<int, char?> answerDict, int gridSize, int wordLength)
    {
        isMoving = true;
        remainingMoves--;
        UIManager.instance.UpdateRemainingMovesText(remainingMoves);
        bool wonGame = CheckBoard(answerWords, answerDict, gridSize, wordLength);
        if (wonGame)
        {
            StartCoroutine(EndGame(wonGame, answerDict));
        }
        else if (remainingMoves == 0)
        {
            for (int i = 0; i < gridSize; i++)
            {
                if (i / wordLength % 2 == 1 && i % 2 == 0) continue;

                GameObject letterObj = gridManagement.grid.transform.GetChild(i).GetChild(0).gameObject;
                letterObj.GetComponent<DraggableLetter>().enabled = false;
            }
            StartCoroutine(EndGame(wonGame, answerDict));
        }
        isMoving = false;
    }

    public bool CheckBoard(string[] answerWords, Dictionary<int, char?> answerDict, int gridSize, int wordLength)
    {
        bool allLettersInCorrectPlace = true;
        for (int i = 0; i < gridSize; i++)
        {
            if (i / wordLength % 2 == 1 && i % 2 == 0) continue;

            GameObject letterObj = gridManagement.grid.transform.GetChild(i).GetChild(0).gameObject;
            char letter = letterObj.GetComponentInChildren<TextMeshProUGUI>().text[0];
            if (letterObj.GetComponent<DraggableLetter>().enabled == false)
            {
                continue;
            }

            if (answerDict[i] == letter)
            {
                letterObj.GetComponentInChildren<Image>().color = greenColor;
                letterObj.GetComponent<DraggableLetter>().enabled = false; // Disable green letters
            }
            else
            {
                allLettersInCorrectPlace = false;
                int[] wordsIndex = gridManagement.GetLetterWordsIndex(i, wordLength);
                bool incorrectPosition = CheckWord(answerWords, letter, wordsIndex[0], wordLength) || CheckWord(answerWords, letter, wordsIndex[1], wordLength);
                if (incorrectPosition)
                {
                    letterObj.GetComponentInChildren<Image>().color = yellowColor;
                }
                else
                {
                    letterObj.GetComponentInChildren<Image>().color = whiteColor;
                }
            }
        }
        return allLettersInCorrectPlace;
    }

    public bool CheckWord(string[] answerWords, char letter, int wordIdx, int wordLength)
    {
        if (wordIdx == -1)
        {
            return false;
        }
        string word = answerWords[wordIdx];
        bool rowWord = wordIdx <= wordLength / 2;

        int letterAppearancesInWord = 0;
        int letterInCorrectPlaceAppearances = 0;

        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == letter)
            {
                letterAppearancesInWord++;
                int gridIdx = rowWord ? wordIdx * wordLength * 2 + i : (wordIdx - (wordLength / 2 + 1)) * 2 + i * wordLength;
                if (word[i] == gridManagement.grid.transform.GetChild(gridIdx).GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text[0])
                {
                    letterInCorrectPlaceAppearances++;
                }
            }
        }
        return letterInCorrectPlaceAppearances < letterAppearancesInWord;
    }

    IEnumerator EndGame(bool won, Dictionary<int, char?> answerDict)
    {
        UserManager.instance.EndGame(won);
        if (won)
        {
            AudioManager.instance.PlaySound("Win");
        }
        else
        {
            AudioManager.instance.PlaySound("Lose");
        }
        yield return new WaitForSeconds(1f);
        Loader.instance.WipeIn();
        yield return new WaitForSeconds(1f);
        UIManager.instance.ShowEndGameCanvas(won, answerDict);
        Loader.instance.WipeOut();
        yield return new WaitForSeconds(1f);
    }
}