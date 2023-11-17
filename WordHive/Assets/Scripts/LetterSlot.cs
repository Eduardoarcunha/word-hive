using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class LetterSlot : MonoBehaviour, IDropHandler
{
    public static event Action OnLetterSwap;

    public void OnDrop(PointerEventData eventData)
    {
        AudioManager.instance.PlaySound("ReleaseDrag");
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableLetter draggableLetter = dropped.GetComponent<DraggableLetter>();
            draggableLetter.parentAfterDrag = transform;
        }
        else
        {
            if (transform.GetChild(0).GetComponent<DraggableLetter>().enabled == false) return;
            GameObject dropped = eventData.pointerDrag;
            StartCoroutine(GoToNewPosition(dropped));
        }
    }

    private IEnumerator GoToNewPosition(GameObject dropped)
    {
        float elapsedTime = 0.0f;
        float totalDuration = .1f;

        DraggableLetter draggableLetter = dropped.GetComponent<DraggableLetter>();

        GameObject current = transform.GetChild(0).gameObject;
        DraggableLetter currentDraggable = current.GetComponent<DraggableLetter>();

        Vector3 startPosition = current.transform.position;
        Vector3 destinyPosition = draggableLetter.parentAfterDrag.transform.position;
        Transform destinyParent = draggableLetter.parentAfterDrag;

        draggableLetter.parentAfterDrag = transform;

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / totalDuration);

            current.transform.position = Vector3.Lerp(startPosition, destinyPosition, progress);
            yield return null;
        }

        current.transform.position = destinyPosition;
        currentDraggable.transform.SetParent(destinyParent);

        yield return new WaitForSeconds(.1f);
        OnLetterSwap?.Invoke();
    }

}
