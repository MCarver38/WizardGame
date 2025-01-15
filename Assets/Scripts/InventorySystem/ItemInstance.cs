[System.Serializable]
public class ItemInstance
{
    public enum Elements { fire, water, ice, air }
    public enum Rarity { common, uncommon, rare, epic }
    
    public int damageAmount;
    public Rarity rarity;
    public Elements elementType;
}
