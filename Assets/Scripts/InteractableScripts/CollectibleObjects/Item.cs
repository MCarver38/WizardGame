using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public int itemMaxStackSize;
    public int quantity;
    public ItemInstance itemInstance;
    
    public delegate void ItemCollected(Item item);
    public static event ItemCollected OnItemCollected;

    private void Awake()
    {
        if (itemData != null)
        {
            InitializeItemData();
        }
    }

    protected void CollectItem()
    {
        OnItemCollected?.Invoke(this);
        Destroy(gameObject);
    }

    private void InitializeItemData()
    {
        itemName = itemData.itemName;
        itemIcon = itemData.icon;
        itemMaxStackSize = itemData.maxStackSize;
        itemDescription = itemData.description;
        quantity = itemData.quantity;
        itemInstance = itemData.itemInstance;
    }

    public abstract void Interact();
}
