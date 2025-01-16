using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public int maxSlots = 24;
    
    public static InventorySystem instance;

    public event Action onInventoryUpdated;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Item.OnItemCollected += AddItem;
    }

    private void OnDisable()
    {
        Item.OnItemCollected -= AddItem;
    } 
    
    public void AddItem(Item item)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.quantity < item.itemMaxStackSize)
            {
                int spaceLeft = item.itemMaxStackSize - slot.quantity;
                int toAdd = Mathf.Min(spaceLeft, item.quantity);
                slot.quantity += toAdd;
                item.quantity -= toAdd;

                if (item.quantity <= 0)
                {
                    onInventoryUpdated?.Invoke();
                    return;
                }
            }
        }

        if (inventorySlots.Count < maxSlots)
        {
            int toAdd = Mathf.Min(item.itemMaxStackSize, item.quantity);
            inventorySlots.Add(new InventorySlot(item.itemData, toAdd));
            item.quantity -= toAdd;
            
            onInventoryUpdated?.Invoke();
        }

        if (item.quantity > 0)
        {
            Debug.Log($"Not enough space for item: {item.name}");
        }
    }
}
