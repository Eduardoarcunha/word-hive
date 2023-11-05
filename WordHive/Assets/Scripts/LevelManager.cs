using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class LevelManager : MonoBehaviour
{
    // private string url = "https://felipesbs.pythonanywhere.com/getGrid?lang=en&level=1";

    private const int GRID_SIZE = 25;
    private const int WORD_LENGTH = 5;

    private string[] answerWords = {"AMBOS", "AROMA", "AMADA", "ARARA", "BROCA", "SEADA"};
    private GameObject grid;
    private Dictionary<int, char?> answerDict = new Dictionary<int, char?>();

    private Color greenColor = new Color32(24, 111, 101, 255);  // #186f65
    private Color yellowColor = new Color32(255, 193, 2, 255);  // #FFC102
    private Color whiteColor = new Color32(190, 191, 173, 255); // #d2e0fb


    void Awake()
    {
        DraggableLetter.OnLetterSlotDrop += CheckBoard;
    }
    
    void Start()
    {
        grid = GameObject.FindWithTag("Grid");
        PopulateAnswerDictionary();
        string randomWord = RandomizeAnsDict();
        SetChilds(randomWord);
        CheckBoard();
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

            grid.transform.GetChild(i).GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = word[j].ToString();
            j++;

        }
    }

    string RandomizeAnsDict()
    {
        string word = "";
        foreach (KeyValuePair<int, char?> entry in answerDict)
        {
            if (entry.Value != null){
                word += entry.Value;
            }
        }
        char[] charArray = word.ToCharArray();

        int n = charArray.Length;
        for (int i = n - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            char temp = charArray[i];
            charArray[i] = charArray[j];
            charArray[j] = temp;
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

    void CheckBoard()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            if (i / WORD_LENGTH % 2 == 1 && i % 2 == 0)
            {
                continue;
            }
            char letter = grid.transform.GetChild(i).GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text[0];
            if (answerDict[i] == letter)
            {
                grid.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().color = greenColor;
            } else
            {
                int[] wordsIndex = GetLetterWordsIndex(i);
                bool incorrectPosition = CheckWord(letter, wordsIndex[0]) || CheckWord(letter, wordsIndex[1]);
                if (incorrectPosition)
                {
                    grid.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().color = yellowColor;
                } else
                {
                    grid.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().color = whiteColor;
                }
            }
        }
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
}