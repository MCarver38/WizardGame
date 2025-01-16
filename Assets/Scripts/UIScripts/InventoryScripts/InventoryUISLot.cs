using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUISlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject droppedObject = eventData.pointerDrag;
            DraggableItem draggableItem = droppedObject.GetComponent<DraggableItem>();
            draggableItem.parentAfterDrag = transform;
        }
    }
}
