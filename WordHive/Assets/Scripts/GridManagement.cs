using System.Collections.Generic;
using System;
using UnityEngine;

using TMPro;

public class GridManagement : MonoBehaviour
{
    public GameObject grid;
    public Dictionary<int, char?> answerDict = new Dictionary<int, char?>();
    private const float RANDOM_LEVEL = .3f;

    public void InitializeGrid(string[] answerWords, int gridSize, int wordLength)
    {
        grid = GameObject.FindWithTag("Grid");
        PopulateAnswerDictionary(answerWords, gridSize, wordLength);
        string randomWord = RandomizeAnsDict();
        SetChilds(randomWord, gridSize, wordLength);
    }

    private void PopulateAnswerDictionary(string[] answerWords, int gridSize, int wordLength)
    {
        for (int i = 0; i < gridSize; i++)
        {
            answerDict.Add(i, DetermineCharacterForIndex(i, answerWords, gridSize, wordLength));
        }
    }

    char? DetermineCharacterForIndex(int index, string[] answerWords, int gridSize, int wordLength)
    {
        if ((index / wordLength) % 2 == 0)
        {
            int rowLetter = index % wordLength;
            return answerWords[index / (wordLength * 2)][rowLetter];
        }

        if (((index / wordLength) % 2 == 1) && (index % 2 != 0))
        {
            int collumnWordsStartIndex = (wordLength / 2 + 1);
            int collumnLetter = index / wordLength;
            return answerWords[collumnWordsStartIndex + (index % wordLength) / 2][collumnLetter];
        }
        return null;
    }


    private void SetChilds(string word, int gridSize, int wordLength)
    {
        int j = 0;
        for (int i = 0; i < gridSize; i++)
        {
            if (i / wordLength % 2 == 1 && i % 2 == 0)
            {
                continue;
            }

            GameObject letter = grid.transform.GetChild(i).GetChild(0).gameObject;
            letter.GetComponentInChildren<TextMeshProUGUI>().text = word[j].ToString();
            j++;
        }
    }

    private string RandomizeAnsDict()
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

    public int[] GetLetterWordsIndex(int index, int wordLength)
    {
        int[] wordsIndex = new int[2];
        Array.Fill(wordsIndex, -1);  // Initialize the array with -1

        if ((index / wordLength) % 2 == 0) // Rows words 
        {
            wordsIndex[0] = index / (wordLength * 2);
        }

        if ((index + index / wordLength) % 2 == 0) // Collumn words
        {
            wordsIndex[1] = (wordLength / 2 + 1) + (index % wordLength) / 2;
        }

        return wordsIndex;
    }
}