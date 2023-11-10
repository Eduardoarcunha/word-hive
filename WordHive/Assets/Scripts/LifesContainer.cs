using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LifesContainer : MonoBehaviour
{
    [SerializeField] private TMP_Text remaingTimeText;
    [SerializeField] private TMP_Text lifesText;


    // Update is called once per frame
    void Update()
    {
        int remainingTimeInSeconds = UserManager.instance.GetRemainingTime();
        int minutes = remainingTimeInSeconds / 60;
        int seconds = remainingTimeInSeconds % 60;
        string remainingTimeFormatted = string.Format("{0:00}:{1:00}", minutes, seconds);

        lifesText.text = UserManager.instance.GetLifes().ToString();
        if (UserManager.instance.GetLifes() == UserManager.MAX_LIVES)
        {
            remaingTimeText.text = " ";
        }
        else
        {
            remaingTimeText.text = remainingTimeFormatted;
        }

    }
}
