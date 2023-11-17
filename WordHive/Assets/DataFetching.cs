using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DataFetching : MonoBehaviour
{
    private WordList wordList;

    public IEnumerator RequestGame(string uri, string[] answerWords)
    {
        Debug.Log("Requesting game");
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Make the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                wordList = JsonUtility.FromJson<WordList>(webRequest.downloadHandler.text);
                for (int i = 0; i < wordList.words.Length; i++)
                {
                    answerWords[i] = wordList.words[i].word;
                }

            }
        }
        gameObject.GetComponent<LevelManager>().StartGame();
    }
}
