using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class LangButtonHandler : MonoBehaviour
{
    public static event Action OnChangeLanguage;

    [SerializeField] private GameObject otherLanguageButton;
    [SerializeField] private string language;

    private Color unselectedColor = new Color32(165, 142, 101, 255);
    private Color selectedColor = new Color32(220, 161, 29, 255);

    private Vector3 unselectedScale = new Vector3(1f, 1f, 1f);
    private Vector3 selectedScale = new Vector3(1.1f, 1.1f, 1.1f);

    private float transitionTime = .3f;

    private TMP_Text buttonText;
    private Image buttonImage;
    private Coroutine btnAnimationCoroutine;


    private void Awake()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
        buttonImage = GetComponentInChildren<Image>();
        // unselectedScale = transform.localScale;
        SetAwakeProps();
    }

    private void SetAwakeProps()
    {
        if (PlayerPrefs.GetString("language") == language)
        {
            transform.localScale = selectedScale;
            if (buttonText != null)
            {
                buttonText.color = selectedColor;
            }
            if (buttonImage != null)
            {
                buttonImage.color = selectedColor;
            }
        }
        else
        {
            transform.localScale = unselectedScale;
            if (buttonText != null)
            {
                buttonText.color = unselectedColor;
            }
            if (buttonImage != null)
            {
                buttonImage.color = unselectedColor;
            }
        }
    }


    public void SelectLanguage(string language)
    {
        if (PlayerPrefs.GetString("language") != language)
        {
            AudioManager.instance.PlaySound("ButtonSound");
            PlayerPrefs.SetString("language", language);
            btnAnimationCoroutine = StartCoroutine(AnimateScaleAndColor(selectedScale, selectedColor));
            otherLanguageButton.GetComponent<LangButtonHandler>().DeselectLanguage();
            OnChangeLanguage?.Invoke();
        }
    }

    public void DeselectLanguage()
    {
        if (btnAnimationCoroutine == null)
        {
            btnAnimationCoroutine = StartCoroutine(AnimateScaleAndColor(unselectedScale, unselectedColor));
        }
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

        btnAnimationCoroutine = null;
    }

    private float EaseInOutQuad(float t)
    {
        return t < 0.5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
    }
}
