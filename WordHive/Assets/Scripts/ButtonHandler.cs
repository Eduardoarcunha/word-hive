using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private Vector3 targetScale = new Vector3(1.1f, 1.1f, 1.1f);
    [SerializeField] private Color targetColor = new Color32(220, 161, 29, 255); // #dca11d
    private float transitionTime = .7f;

    private Vector3 originalScale;
    private Color originalColor;
    private TMP_Text buttonText;
    private Image buttonImage;
    private Coroutine btnAnimationCoroutine;


    private void Awake()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
        buttonImage = GetComponentInChildren<Image>();
        originalScale = transform.localScale;
        CacheOriginalColor();
    }

    private void CacheOriginalColor()
    {
        if (buttonText != null)
        {
            originalColor = buttonText.color;
        }
        else if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }
    }

    public void PlayGame()
    {
        if (btnAnimationCoroutine == null)
        {
            if (UserManager.instance.GetLifes() > 0)
            {
                Loader.instance.LoadScene(1);
                btnAnimationCoroutine = StartCoroutine(SimpleBtnAnimation());
            }
            else
            {
                // AdsManager.instance.ShowAdsPanel();
                Debug.Log("No lifes");
            }
        }
    }

    public void ReturnToMenu()
    {
        if (btnAnimationCoroutine == null)
        {
            Loader.instance.LoadScene(0);
            btnAnimationCoroutine = StartCoroutine(SimpleBtnAnimation());
        }
    }

    public void ClosePopup()
    {
        if (btnAnimationCoroutine == null)
        {
            AdsManager.instance.HideAdsPanel();
            // btnAnimationCoroutine = StartCoroutine(SimpleBtnAnimation(true));
        }
    }


    private IEnumerator SimpleBtnAnimation()
    {
        AudioManager.instance.PlaySound("ButtonSound");
        yield return StartCoroutine(AnimateScaleAndColor(targetScale, targetColor));
        yield return StartCoroutine(AnimateScaleAndColor(originalScale, originalColor));

        btnAnimationCoroutine = null;


    }

    private IEnumerator AnimateScaleAndColor(Vector3 scale, Color color)
    {
        Vector3 startScale = transform.localScale;
        Color startColor = buttonText != null ? buttonText.color : buttonImage.color;
        float elapsedTime = 0;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / transitionTime;
            float easedProgress = EaseInOutQuad(progress);

            transform.localScale = Vector3.Lerp(startScale, scale, easedProgress);
            if (buttonText != null)
            {
                buttonText.color = Color.Lerp(startColor, color, easedProgress);
            }
            if (buttonImage != null)
            {
                buttonImage.color = Color.Lerp(startColor, color, easedProgress);
            }

            yield return null;
        }

        transform.localScale = scale;
        if (buttonText != null)
        {
            buttonText.color = color;
        }
        if (buttonImage != null)
        {
            buttonImage.color = color;
        }

    }

    private float EaseInOutQuad(float t)
    {
        return t < 0.5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
    }
}
