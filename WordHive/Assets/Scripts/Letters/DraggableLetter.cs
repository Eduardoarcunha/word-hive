using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class DraggableLetter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    private float scaleTime = .1f;
    private float baseScale = 1f;
    private float selectedScale = 1.3f;
    private bool isChecking = false;


    void Awake()
    {
        LevelManager.OnCheckBoard += OnCheckBoardEvent;
    }

    private void OnCheckBoardEvent(bool isCorrect)
    {
        if (isCorrect)
        {
            isChecking = true;
            image.raycastTarget = false;
        }
        else
        {
            isChecking = false;
            image.raycastTarget = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        AudioManager.instance.PlaySound("Drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.parent.parent.parent.parent.parent);
        transform.SetAsLastSibling();
        StartCoroutine(ScaleOverTime(new Vector3(selectedScale, selectedScale, selectedScale)));
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        AudioManager.instance.PlaySound("ReleaseDrag");
        transform.SetParent(parentAfterDrag);
        StartCoroutine(ReturnToOriginalPosition());
    }

    private IEnumerator ScaleOverTime(Vector3 target)
    {
        float elapsedTime = 0;
        while (elapsedTime < scaleTime)
        {
            elapsedTime += Time.deltaTime;
            float nextScale = baseScale + (elapsedTime / scaleTime) * (selectedScale - baseScale);
            transform.localScale = new Vector3(nextScale, nextScale, nextScale);
            yield return null;
        }
        transform.localScale = new Vector3(selectedScale, selectedScale, selectedScale);
    }


    private IEnumerator ReturnToOriginalPosition()
    {
        // if (!isChecking)
        // {
        //     AudioManager.instance.PlaySound("ReleaseDrag");
        // }
        float elapsedTime = 0.0f;
        float totalDuration = .15f;

        Vector3 startPosition = transform.position;
        Vector3 destinyPosition = parentAfterDrag.position;

        Vector3 startScale = transform.localScale;
        Vector3 endScale = new Vector3(baseScale, baseScale, baseScale);

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / totalDuration);

            transform.position = Vector3.Lerp(startPosition, destinyPosition, progress);
            transform.localScale = Vector3.Lerp(startScale, endScale, progress);
            yield return null;
        }

        transform.position = destinyPosition;
        if (!isChecking)
        {
            image.raycastTarget = true;
        }
        transform.localScale = endScale;
    }

    void OnDestroy()
    {
        LevelManager.OnCheckBoard -= OnCheckBoardEvent;
    }
}
