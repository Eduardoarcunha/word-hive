using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class ButtonHandler : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(1.1f, 1.1f, 1.1f);
    private Vector3 originalScale;
    public Color targetColor = new Color32(220, 161, 29, 255);
    private Color originalColor;
    private TMP_Text buttonText;

    private Image buttonImage;
    private Color originalImageColor;

    private float scaleSpeed = .5f;
    private float colorChangeSpeed = .5f;
    private float transitionTime = .5f;

    private Coroutine startGameCoroutine;

    private void Start()
    {
        originalScale = transform.localScale;
        buttonText = GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            originalColor = buttonText.color;
        }
        buttonImage = GetComponentInChildren<Image>();
        if (buttonImage != null)
        {
            originalImageColor = buttonImage.color;
        }
    }

    public void PlayGame()
    {
        if (startGameCoroutine == null)
        {
            startGameCoroutine = StartCoroutine(PlayGameCoroutine());
        }    
    }

    public IEnumerator PlayGameCoroutine()
    {
        float elapsedTime = 0;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;

            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, elapsedTime * scaleSpeed);
            if (buttonText != null)
            {
                buttonText.color = Color.Lerp(buttonText.color, targetColor, elapsedTime * colorChangeSpeed);
            }

            if (buttonImage != null && gameObject.CompareTag("ImageButton"))
            {
                buttonImage.color = Color.Lerp(buttonImage.color, targetColor, elapsedTime * colorChangeSpeed);
            }

            yield return null;
        }

        transform.localScale = targetScale;
        if (buttonText != null)
        {
            buttonText.color = targetColor;
        }

        if (buttonImage != null && gameObject.CompareTag("ImageButton"))
        {
            buttonImage.color = targetColor;
        }

        SceneManager.LoadScene("GameScene");
        
    }
}
