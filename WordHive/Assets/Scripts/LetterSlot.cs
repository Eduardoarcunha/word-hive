using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LetterSlot : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableLetter draggableLetter = dropped.GetComponent<DraggableLetter>();
            draggableLetter.parentAfterDrag = transform;
        }
        else
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableLetter draggableLetter = dropped.GetComponent<DraggableLetter>();

            GameObject current = transform.GetChild(0).gameObject;
            DraggableLetter currentDraggable = current.GetComponent<DraggableLetter>();

            currentDraggable.transform.SetParent(draggableLetter.parentAfterDrag);
            draggableLetter.parentAfterDrag = transform;
        }
        

    }
}
