using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{

    public static MenuUIManager instance;

    [SerializeField] private GameObject optionsPanel;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowOptionsCanvas()
    {
        optionsPanel.SetActive(true);
    }

    public void HideOptionsCanvas()
    {
        optionsPanel.SetActive(false);
    }
}
