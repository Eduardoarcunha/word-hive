using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;


public class LevelManager : MonoBehaviour
{
    private const int GRID_SIZE = 25;
    private const int WORD_LENGTH = 5;

    private string[] answerWords = {"ambos", "aroma", "amada", "arara", "broca", "seada"};
    private GameObject grid;
    private Dictionary<int, char?> answerDict = new Dictionary<int, char?>();
    
    void Start()
    {
        grid = GameObject.FindWithTag("Grid");
        PopulateAnswerDictionary();
        string randomWord = RandomizeAnsDict();
        Debug.Log(randomWord);
        Debug.Log(randomWord.Length);
        SetChilds(randomWord);
        
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
            int j = Random.Range(0, i + 1);
            char temp = charArray[i];
            charArray[i] = charArray[j];
            charArray[j] = temp;
        }

        return new string(charArray);
    }

}