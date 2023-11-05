using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingContainer : MonoBehaviour
{
    private string[] texts;
    private int index = 0;
    private TMP_Text text;

    private void Start()
    {
        texts = new string[] { "LOADING", "LOADING.", "LOADING..", "LOADING..." };
        text = GetComponentInChildren<TMP_Text>();
    }

    private void ChangeText()
    {
        index++;
        if (index >= texts.Length)
        {
            index = 0;
        }
        text.text = texts[index];
    }
}
