using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Linq;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public static Action<bool> OnCheckBoard;

    private const int GRID_SIZE = 25;
    private const int WORD_LENGTH = 5;

    private const int TOTAL_MOVES = 15;
    private int remainingMoves;

    private string[] answerWords = { "AMBOS", "AROMA", "AMADA", "ARARA", "BROCA", "SEADA" };

    private GridManagement gridManagement;
    private GameLogic gameLogic;
    private DataFetching dataFetching;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        gridManagement = GetComponent<GridManagement>();
        gameLogic = GetComponent<GameLogic>();
        dataFetching = GetComponent<DataFetching>();

        LetterSlot.OnLetterSwap += OnLetterSwapEvent;
    }

    void Start()
    {
        int wonLastGame = PlayerPrefs.GetInt("wonLastGame");
        int id = PlayerPrefs.GetInt("id");
        string url = "https://felipesbs.pythonanywhere.com/getGrid?win=" + wonLastGame + "&id=" + id + "&language=en";

        StartCoroutine(dataFetching.RequestGame(url, answerWords));
    }


    public void StartGame()
    {
        gridManagement.InitializeGrid(answerWords, GRID_SIZE, WORD_LENGTH);
        gameLogic.CheckBoard(answerWords, gridManagement.answerDict, GRID_SIZE, WORD_LENGTH);
        remainingMoves = TOTAL_MOVES;
        UIManager.instance.UpdateRemainingMovesText(remainingMoves);
        Loader.instance.WipeOut();
    }


    void OnLetterSwapEvent()
    {
        OnCheckBoard?.Invoke(true);
        gameLogic.MoveMade(answerWords, gridManagement.answerDict, GRID_SIZE, WORD_LENGTH);
        StartCoroutine(DelayedEndOfCheckBoard()); // Start a coroutine for the delay
    }

    IEnumerator DelayedEndOfCheckBoard()
    {
        yield return new WaitForSeconds(.04f);
        OnCheckBoard?.Invoke(false);
    }


    void OnDestroy()
    {
        LetterSlot.OnLetterSwap -= OnLetterSwapEvent;
    }
}