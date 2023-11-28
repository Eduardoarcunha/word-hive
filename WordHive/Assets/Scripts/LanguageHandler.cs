using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageHandler : MonoBehaviour
{
    [SerializeField] private string pt_text;
    [SerializeField] private string en_text;

    void Awake()
    {
        LangButtonHandler.OnChangeLanguage += SwitchText;

        if (PlayerPrefs.GetString("language") == "pt")
        {
            GetComponent<TMPro.TMP_Text>().SetText(pt_text);
        }
        else
        {
            GetComponent<TMPro.TMP_Text>().SetText(en_text);
        }
    }

    void SwitchText()
    {
        if (PlayerPrefs.GetString("language") == "pt")
        {
            GetComponent<TMPro.TMP_Text>().SetText(pt_text);
        }
        else
        {
            GetComponent<TMPro.TMP_Text>().SetText(en_text);
        }
    }

    void OnDestroy()
    {
        LangButtonHandler.OnChangeLanguage -= SwitchText;
    }
}
