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

    public static event Action OnLetterSlotDrop;


    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        StartCoroutine(ReturnToOriginalPosition());
        // transform.SetParent(parentAfterDrag);
        // image.raycastTarget = true;
        // OnLetterSlotDrop?.Invoke();
    }


    private IEnumerator ReturnToOriginalPosition()
    {
        float speed = 5000.0f; // Units per second.
        Vector3 destinyPosition = parentAfterDrag.position;

        while (Vector3.Distance(transform.position, destinyPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destinyPosition, speed * Time.deltaTime);
            yield return null;
        }

        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        OnLetterSlotDrop?.Invoke();    
    }
}
