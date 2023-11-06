using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Loader : MonoBehaviour
{
    public static Loader instance;
    public Animator animator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
        } else {
            Destroy(gameObject);
        }
    }

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        animator.SetTrigger("In");
        yield return new WaitForSeconds(2f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        
        while (!operation.isDone)
        {
            yield return null;
        }

    }

    public void WipeOut()
    {
        animator.SetTrigger("Out");
    }
}
