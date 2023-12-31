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
    public GameObject hiveImage;

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

    void Start()
    {
        // get current scene
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex == 0)
        {
            AudioManager.instance.PlaySound("MenuMusic");
        }
    }

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        WipeIn();
        yield return new WaitForSeconds(1f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);


        while (!operation.isDone)
        {
            yield return null;
        }


        if (sceneId != 1)
        {
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !animator.IsInTransition(0));
            WipeOut();
            StartCoroutine(AudioManager.instance.SetVolume("MenuMusic", .2f));
        }
        else
        {
            StartCoroutine(AudioManager.instance.SetVolume("MenuMusic", .1f));
        }
    }

    public IEnumerator WaitLoaderAnimation()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !animator.IsInTransition(0));
    }

    public void WipeIn()
    {
        animator.SetTrigger("In");
    }

    public void WipeOut()
    {
        animator.SetTrigger("Out");
    }

    public IEnumerator LoadOptions()
    {
        WipeIn();
        yield return new WaitForSeconds(1f);
        MenuUIManager.instance.ShowOptionsCanvas();
        WipeOut();
    }

}
