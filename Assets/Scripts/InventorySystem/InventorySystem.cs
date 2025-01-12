using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public int maxSlots = 30;

    public bool AddItem(ItemInstance item, int quantity, ItemData itemData)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.quantity < itemData.maxStackSize)
            {
                int spaceLeft = itemData.maxStackSize - slot.quantity;
                int toAdd = Mathf.Min(spaceLeft, quantity);
                slot.quantity += toAdd;
                quantity -= toAdd;

                if (quantity <= 0) return true;
            }
        }

        if (inventorySlots.Count < maxSlots)
        {
            int toAdd = Mathf.Min(itemData.maxStackSize, quantity);
            inventorySlots.Add(new InventorySlot(item, toAdd));
            quantity -= toAdd;
        }

        return quantity == 0;
    }
}
