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
    private const int GRID_SIZE = 25;
    private const int WORD_LENGTH = 5;
    private const float RANDOM_LEVEL = .3f;

    private const int TOTAL_MOVES = 15;
    private int remainingMoves;


    private string[] answerWords = { "AMBOS", "AROMA", "AMADA", "ARARA", "BROCA", "SEADA" };


    private GridManagement gridManagement;
    private GameLogic gameLogic;
    private DataFetching dataFetching;


    void Awake()
    {
        // DraggableLetter.OnLetterSlotDrop += MoveMade;
        gridManagement = GetComponent<GridManagement>();
        gameLogic = GetComponent<GameLogic>();
        dataFetching = GetComponent<DataFetching>();

        DraggableLetter.OnLetterSlotDrop += OnLetterSlotEvent;
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
        Debug.Log(answerWords);
        gridManagement.InitializeGrid(answerWords, GRID_SIZE, WORD_LENGTH);
        gameLogic.CheckBoard(answerWords, gridManagement.answerDict, GRID_SIZE, WORD_LENGTH);
        remainingMoves = TOTAL_MOVES;
        UIManager.instance.UpdateRemainingMovesText(remainingMoves);
        Loader.instance.WipeOut();
    }

    void OnLetterSlotEvent()
    {
        gameLogic.MoveMade(answerWords, gridManagement.answerDict, GRID_SIZE, WORD_LENGTH);
    }


    void OnDestroy()
    {
        DraggableLetter.OnLetterSlotDrop -= OnLetterSlotEvent;
    }
}