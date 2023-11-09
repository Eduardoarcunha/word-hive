using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static UserManager instance;
    private int id;
    private int wonGames;
    private int totalGames;
    private int lives;

    private int currentTime;
    private int lastLifeGainedTime;

    const int LIFE_TIMER = 60 * 60 / 2;
    const int MAX_LIVES = 5;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        // PlayerPrefs.DeleteAll();
        // PlayerPrefs.SetInt("lives", 3);
    }

    void Start()
    {
        id = PlayerPrefs.GetInt("id");
        wonGames = PlayerPrefs.GetInt("wonGames");
        totalGames = PlayerPrefs.GetInt("totalGames");
        lives = PlayerPrefs.GetInt("lives");

        lastLifeGainedTime = PlayerPrefs.GetInt("lastLifeGainedTime");
        currentTime = GetCurrentTimeInSeconds();

        Debug.Log("current time: " + currentTime);
        Debug.Log("last life gained time: " + lastLifeGainedTime);
        Debug.Log("lives: " + lives);

        CheckLifesAtStart();
    }

    void Update()
    {
        currentTime = GetCurrentTimeInSeconds();
        if (currentTime - lastLifeGainedTime > LIFE_TIMER)
        {
            if (lives < MAX_LIVES)
            {
                AddLife();
            }
            lastLifeGainedTime = currentTime;
            PlayerPrefs.SetInt("lastLifeGainedTime", lastLifeGainedTime);
        }
    }

    void CheckLifesAtStart()
    {
        int newLives = Mathf.Min(MAX_LIVES - lives, (currentTime - lastLifeGainedTime) / LIFE_TIMER);
        if (newLives > 0)
        {
            Debug.Log("Adding " + newLives + " lives");
            for (int i = 0; i < newLives; i++)
            {
                AddLife();
            }
            lastLifeGainedTime = currentTime;
            PlayerPrefs.SetInt("lastLifeGainedTime", lastLifeGainedTime);
        }
    }


    private int GetCurrentTimeInSeconds()
    {
        DateTime currentTime = WorldTimeAPI.instance.GetCurrentDateTime();
        return currentTime.Hour * 3600 + currentTime.Minute * 60 + currentTime.Second;
    }

    private void AddLife()
    {
        lives++;
        PlayerPrefs.SetInt("lives", lives);
    }

    public int GetRemainingTime()
    {
        int remainingTime = LIFE_TIMER - (currentTime - lastLifeGainedTime);
        return remainingTime;
    }

    public void SetId(int id)
    {
        this.id = id;
        PlayerPrefs.SetInt("id", id);
    }

    public void EndGame(bool won)
    {
        totalGames++;
        if (won)
        {
            wonGames++;
        }
        PlayerPrefs.SetInt("wonGames", wonGames);
        PlayerPrefs.SetInt("totalGames", totalGames);
    }

    public void PlayGame()
    {
        lives--;
        PlayerPrefs.SetInt("lives", lives);
    }

    public int GetLives()
    {
        return lives;
    }


    private void OnApplicationQuit()
    {
        // PlayerPrefs.SetInt("lastRecordedTime", currentTime);
    }
}
