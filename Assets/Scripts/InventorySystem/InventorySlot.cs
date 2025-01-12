using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemInstance Item;
    public int quantity;

    public InventorySlot(ItemInstance data, int qty)
    {
        Item = data;
        quantity = qty;
    }
}
