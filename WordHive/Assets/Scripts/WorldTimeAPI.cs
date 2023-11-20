using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;

public class WorldTimeAPI : MonoBehaviour
{
    #region Singleton class: WorldTimeAPI

    public static WorldTimeAPI instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    // json container
    [Serializable]
    private struct TimeData
    {
        public string datetime;
    }

    const string API_URL = "https://worldtimeapi.org/api/ip";
    [HideInInspector] public bool IsTimeLoaded = false;
    private DateTime currentDateTime = DateTime.Now;
    private int retryCount = 0;
    private const int maxRetries = 3; // Max number of retries
    private const float retryDelay = 2.0f; // Delay between retries in seconds

    void Start()
    {
        StartCoroutine(GetRealDateTimeFromAPI());
    }

    public DateTime GetCurrentDateTime()
    {
        return currentDateTime.AddSeconds(Time.realtimeSinceStartup);
    }

    IEnumerator GetRealDateTimeFromAPI()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(API_URL))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
                retryCount++;
                if (retryCount <= maxRetries)
                {
                    Debug.Log("Retrying... Attempt: " + retryCount);
                    yield return new WaitForSeconds(retryDelay);
                    StartCoroutine(GetRealDateTimeFromAPI());
                }
            }
            else
            {
                TimeData timeData = JsonUtility.FromJson<TimeData>(webRequest.downloadHandler.text);
                currentDateTime = DateTime.Parse(timeData.datetime);
                IsTimeLoaded = true;
                Debug.Log("Time successfully loaded.");
                retryCount = 0; // Reset retry count after a successful attempt
            }
        }
    }
}
