using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;

    void Awake()
    {
        levelText.text = "Level " + PlayerPrefs.GetInt("level").ToString();
    }
}
