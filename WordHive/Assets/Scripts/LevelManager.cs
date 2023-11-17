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

    private WordList wordList;
    private string[] answerWords = { "AMBOS", "AROMA", "AMADA", "ARARA", "BROCA", "SEADA" };
    private GameObject grid;
    private Dictionary<int, char?> answerDict = new Dictionary<int, char?>();

    private Color greenColor = new Color32(24, 111, 101, 255);  // #186f65
    private Color yellowColor = new Color32(255, 193, 2, 255);  // #FFC102
    private Color whiteColor = new Color32(190, 191, 173, 255); // #d2e0fb


    void Awake()
    {
        LetterSlot.OnLetterSwap += MoveMade;
    }

    void Start()
    {
        int wonLastGame = PlayerPrefs.GetInt("wonLastGame");
        int id = PlayerPrefs.GetInt("id");
        string url = "https://felipesbs.pythonanywhere.com/getGrid?win=" + wonLastGame + "&id=" + id + "&language=en";

        StartCoroutine(RequestGame(url));
    }

    private IEnumerator RequestGame(string uri)
    {
        Debug.Log("Requesting game");
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Make the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                wordList = JsonUtility.FromJson<WordList>(webRequest.downloadHandler.text);
                for (int i = 0; i < wordList.words.Length; i++)
                {
                    answerWords[i] = wordList.words[i].word;
                }

                StartGame();
            }
        }
    }

    void StartGame()
    {
        grid = GameObject.FindWithTag("Grid");
        PopulateAnswerDictionary();
        string randomWord = RandomizeAnsDict();
        SetChilds(randomWord);
        CheckBoard();
        remainingMoves = TOTAL_MOVES;
        UIManager.instance.UpdateRemainingMovesText(remainingMoves);
        Loader.instance.WipeOut();
    }

    void PopulateAnswerDictionary()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            answerDict.Add(i, DetermineCharacterForIndex(i));
        }
    }

    char? DetermineCharacterForIndex(int index)
    {
        if ((index / WORD_LENGTH) % 2 == 0)
        {
            int rowLetter = index % WORD_LENGTH;
            return answerWords[index / (WORD_LENGTH * 2)][rowLetter];
        }

        if (((index / WORD_LENGTH) % 2 == 1) && (index % 2 != 0))
        {
            int collumnWordsStartIndex = (WORD_LENGTH / 2 + 1);
            int collumnLetter = index / WORD_LENGTH;
            return answerWords[collumnWordsStartIndex + (index % WORD_LENGTH) / 2][collumnLetter];
        }

        return null;
    }

    void SetChilds(string word)
    {
        int j = 0;
        for (int i = 0; i < GRID_SIZE; i++)
        {
            if (i / WORD_LENGTH % 2 == 1 && i % 2 == 0)
            {
                continue;
            }

            GameObject letter = grid.transform.GetChild(i).GetChild(0).gameObject;
            letter.GetComponentInChildren<TextMeshProUGUI>().text = word[j].ToString();
            j++;
        }
    }

    string RandomizeAnsDict()
    {
        string word = "";
        foreach (KeyValuePair<int, char?> entry in answerDict)
        {
            if (entry.Value != null)
            {
                word += entry.Value;
            }
        }
        char[] charArray = word.ToCharArray();

        int n = charArray.Length;
        for (int i = n - 1; i > 0; i--)
        {
            if (UnityEngine.Random.Range(0f, 1f) < RANDOM_LEVEL)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                char temp = charArray[i];
                charArray[i] = charArray[j];
                charArray[j] = temp;
            }
        }

        return new string(charArray);
    }

    int[] GetLetterWordsIndex(int index)
    {
        int[] wordsIndex = new int[2];
        Array.Fill(wordsIndex, -1);  // Initialize the array with -1

        if ((index / WORD_LENGTH) % 2 == 0) // Rows words 
        {
            wordsIndex[0] = index / (WORD_LENGTH * 2);
        }

        if ((index + index / WORD_LENGTH) % 2 == 0) // Collumn words
        {
            wordsIndex[1] = (WORD_LENGTH / 2 + 1) + (index % WORD_LENGTH) / 2;
        }

        return wordsIndex;
    }


    public void MoveMade()
    {
        remainingMoves--;
        UIManager.instance.UpdateRemainingMovesText(remainingMoves);
        bool wonGame = CheckBoard();
        if (wonGame)
        {
            StartCoroutine(EndGame(wonGame));
        }
        else if (remainingMoves == 0)
        {
            StartCoroutine(EndGame(wonGame));
        }
    }

    bool CheckBoard()
    {
        bool allLettersInCorrectPlace = true;
        for (int i = 0; i < GRID_SIZE; i++)
        {
            if (i / WORD_LENGTH % 2 == 1 && i % 2 == 0) continue;

            GameObject letterObj = grid.transform.GetChild(i).GetChild(0).gameObject;
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
                int[] wordsIndex = GetLetterWordsIndex(i);
                bool incorrectPosition = CheckWord(letter, wordsIndex[0]) || CheckWord(letter, wordsIndex[1]);
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

    bool CheckWord(char letter, int wordIdx)
    {
        if (wordIdx == -1)
        {
            return false;
        }
        string word = answerWords[wordIdx];
        bool rowWord = wordIdx <= WORD_LENGTH / 2;

        int letterAppearancesInWord = 0;
        int letterInCorrectPlaceAppearances = 0;

        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == letter)
            {
                letterAppearancesInWord++;
                int gridIdx = rowWord ? wordIdx * WORD_LENGTH * 2 + i : (wordIdx - (WORD_LENGTH / 2 + 1)) * 2 + i * WORD_LENGTH;
                if (word[i] == grid.transform.GetChild(gridIdx).GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text[0])
                {
                    letterInCorrectPlaceAppearances++;
                }
            }
        }
        return letterInCorrectPlaceAppearances < letterAppearancesInWord;
    }

    private IEnumerator EndGame(bool won)
    {
        UserManager.instance.EndGame(won);
        Loader.instance.WipeIn();
        yield return new WaitForSeconds(1f);
        UIManager.instance.ShowEndGameCanvas(won, answerDict);
        Loader.instance.WipeOut();
        yield return new WaitForSeconds(1f);
    }

    void OnDestroy()
    {
        LetterSlot.OnLetterSwap -= MoveMade;
    }
}