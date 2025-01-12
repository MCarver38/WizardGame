using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data/Create New Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    [TextArea]
    public string description;

    public int maxStackSize;
}
