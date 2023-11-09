using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LivesContainer : MonoBehaviour
{
    [SerializeField] private TMP_Text remaingTimeText;
    [SerializeField] private TMP_Text livesText;


    // Update is called once per frame
    void Update()
    {
        int remainingTimeInSeconds = UserManager.instance.GetRemainingTime();
        int minutes = remainingTimeInSeconds / 60;
        int seconds = remainingTimeInSeconds % 60;
        string remainingTimeFormatted = string.Format("{0:00}:{1:00}", minutes, seconds);

        livesText.text = UserManager.instance.GetLives().ToString();

        remaingTimeText.text = remainingTimeFormatted;
    }
}
