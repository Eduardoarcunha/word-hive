using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static UserManager instance;

    private WorldTimeAPI worldTimeAPI;
    private IpAPI ipAPI;

    private int id;
    private int wonGames;
    private int totalGames;
    private int lifes;

    private int currentTime;
    private int lastLifeGainedTime;

    const int LIFE_COOLDOWN = 60 * 60 / 2; // 30 minutes
    public const int MAX_LIVES = 3;

    // PlayerPrefs keys
    private const string LifesKey = "lifes";
    private const string IdKey = "id";
    private const string WonGamesKey = "wonGames";
    private const string TotalGamesKey = "totalGames";
    private const string LastLifeGainedTimeKey = "lastLifeGainedTime";
    private const string Level = "level";
    private const string Language = "language";


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
        worldTimeAPI = GetComponent<WorldTimeAPI>();
        ipAPI = GetComponent<IpAPI>();

        // PlayerPrefs.DeleteAll();
        // PlayerPrefs.SetInt("lifes", 0);

        InitializeUserData();
    }

    void Start()
    {
        currentTime = GetCurrentTimeInSeconds();
        CheckLifesAtStart();
    }

    void Update()
    {
        currentTime = GetCurrentTimeInSeconds();
        if (currentTime - lastLifeGainedTime > LIFE_COOLDOWN)
        {
            if (lifes < MAX_LIVES)
            {
                IncreaseLife();
            }
            lastLifeGainedTime = currentTime;
            PlayerPrefs.SetInt("lastLifeGainedTime", lastLifeGainedTime);
        }
    }

    private void InitializeUserData()
    {
        id = PlayerPrefs.GetInt(IdKey, 0);
        if (id == 0) // New user
        {
            ipAPI.SetId();
            ResetGameData();
        }
        LoadGameData();
    }

    private void ResetGameData()
    {
        PlayerPrefs.SetInt(WonGamesKey, 0);
        PlayerPrefs.SetInt(TotalGamesKey, 0);
        PlayerPrefs.SetInt(LifesKey, 3);
        PlayerPrefs.SetInt(LastLifeGainedTimeKey, GetCurrentTimeInSeconds());
        PlayerPrefs.SetInt(Level, 1);
        PlayerPrefs.SetString(Language, "en");
    }

    private void LoadGameData()
    {
        wonGames = PlayerPrefs.GetInt(WonGamesKey);
        totalGames = PlayerPrefs.GetInt(TotalGamesKey);
        lifes = PlayerPrefs.GetInt(LifesKey);
        if (lifes > MAX_LIVES) lifes = MAX_LIVES;
        PlayerPrefs.SetInt(LifesKey, lifes);
        lastLifeGainedTime = PlayerPrefs.GetInt(LastLifeGainedTimeKey, GetCurrentTimeInSeconds());
    }

    void CheckLifesAtStart()
    {
        int newLives = Mathf.Min(lifes, (currentTime - lastLifeGainedTime) / LIFE_COOLDOWN);
        if (newLives > 0)
        {
            for (int i = 0; i < newLives; i++)
            {
                IncreaseLife();
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
        DateTime currentTime = worldTimeAPI.GetCurrentDateTime();
        return currentTime.Hour * 3600 + currentTime.Minute * 60 + currentTime.Second;
    }


    public int GetRemainingTime()
    {
        return LIFE_COOLDOWN - (currentTime - lastLifeGainedTime);
    }

    public void IncreaseLife()
    {
        Debug.Log("Increase life");
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
            PlayerPrefs.SetInt(Level, PlayerPrefs.GetInt(Level) + 1);
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

    public int GetLevel()
    {
        return PlayerPrefs.GetInt(Level);
    }

}
