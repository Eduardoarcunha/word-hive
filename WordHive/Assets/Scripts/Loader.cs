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

        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        WipeIn();
        yield return new WaitForSeconds(4f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);


        while (!operation.isDone)
        {
            yield return null;
        }

        if (sceneId != 1)
        {
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !animator.IsInTransition(0));
            WipeOut();
        }
    }

    public void WipeIn()
    {
        animator.SetTrigger("In");
    }

    public void WipeOut()
    {
        animator.SetTrigger("Out");
    }
}
