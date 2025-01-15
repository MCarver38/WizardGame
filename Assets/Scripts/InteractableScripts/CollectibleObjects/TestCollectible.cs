using UnityEngine;

public class TestCollectible : Item, IInteractable
{
    [SerializeField] private string collectibleName;
    [SerializeField] private ScriptableObject collectibleObject;
    
    public override void Interact()
    {
        Debug.Log($"Picked up {collectibleName}");
        Destroy(gameObject);
        
        CollectItem();
    }

    public string GetInteractionPrompt()
    {
        return $"Pick up {collectibleName}";
    }
}
