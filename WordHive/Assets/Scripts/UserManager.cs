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
    private int lifes;

    private int currentTime;
    private int lastLifeGainedTime;

    const int LIFE_TIMER = 60 * 60 / 2;
    public const int MAX_LIVES = 5;


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
        // PlayerPrefs.SetInt("lifes", 0);
    }

    void Start()
    {
        id = PlayerPrefs.GetInt("id");
        if (id == 0) // New user
        {
            id = UnityEngine.Random.Range(1, 1000000);
            PlayerPrefs.SetInt("id", id);
            PlayerPrefs.SetInt("wonLastGame", 1);
            PlayerPrefs.SetInt("wonGames", 0);
            PlayerPrefs.SetInt("totalGames", 0);
            PlayerPrefs.SetInt("currentSequence", 0);
            PlayerPrefs.SetInt("maxSequence", 0);
        }
        wonGames = PlayerPrefs.GetInt("wonGames");
        totalGames = PlayerPrefs.GetInt("totalGames");
        lifes = PlayerPrefs.GetInt("lifes");

        lastLifeGainedTime = PlayerPrefs.GetInt("lastLifeGainedTime");
        currentTime = GetCurrentTimeInSeconds();


        CheckLifesAtStart();
    }

    void Update()
    {
        currentTime = GetCurrentTimeInSeconds();
        if (currentTime - lastLifeGainedTime > LIFE_TIMER)
        {
            if (lifes < MAX_LIVES)
            {
                IncreaseLife();
            }
            lastLifeGainedTime = currentTime;
            PlayerPrefs.SetInt("lastLifeGainedTime", lastLifeGainedTime);
        }
    }

    void CheckLifesAtStart()
    {
        int newLives = Mathf.Min(MAX_LIVES - lifes, (currentTime - lastLifeGainedTime) / LIFE_TIMER);
        if (newLives > 0)
        {
            for (int i = 0; i < newLives; i++)
            {
                if (lifes < MAX_LIVES)
                {
                    IncreaseLife();
                }
            }
            lastLifeGainedTime = currentTime;
            PlayerPrefs.SetInt("lastLifeGainedTime", lastLifeGainedTime);
        }

        if (currentTime < lastLifeGainedTime)
        {
            for (int i = 0; i < 5; i++)
            {
                IncreaseLife();
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

    public int GetRemainingTime()
    {
        int remainingTime = LIFE_TIMER - (currentTime - lastLifeGainedTime);
        return remainingTime;
    }

    public void IncreaseLife()
    {
        lifes++;
        lifes = Mathf.Min(lifes, MAX_LIVES);
        PlayerPrefs.SetInt("lifes", lifes);
    }

    public void DecreaseLifes()
    {
        lifes--;
        lifes = Mathf.Max(lifes, 0);
        PlayerPrefs.SetInt("lifes", lifes);
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
            PlayerPrefs.SetInt("wonLastGame", 1);
            PlayerPrefs.SetInt("currentSequence", PlayerPrefs.GetInt("currentSequence") + 1);
            PlayerPrefs.SetInt("maxSequence", Mathf.Max(PlayerPrefs.GetInt("maxSequence"), PlayerPrefs.GetInt("currentSequence")));
        }
        else
        {
            DecreaseLifes();
            PlayerPrefs.SetInt("wonLastGame", 0);
            PlayerPrefs.SetInt("currentSequence", 0);
        }
        PlayerPrefs.SetInt("wonGames", wonGames);
        PlayerPrefs.SetInt("totalGames", totalGames);
    }

    public int GetLifes()
    {
        return lifes;
    }

}
