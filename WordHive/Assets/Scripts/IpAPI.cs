using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class IpAPI : MonoBehaviour
{
    private Id id;

    public IEnumerator RequestId()
    {
        Debug.Log("Requesting id");
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://felipesbs.pythonanywhere.com/getID"))
        {
            // Make the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Id request with success");
                id = JsonUtility.FromJson<Id>(webRequest.downloadHandler.text);
                PlayerPrefs.SetInt("id", int.Parse(id.id));
            }
        }
    }

    public void SetId()
    {
        StartCoroutine(RequestId());
    }
}