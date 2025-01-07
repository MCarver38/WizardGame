using UnityEngine;

public class TestCollectible : MonoBehaviour, IInteractable
{
    [SerializeField] private string collectibleName;
    
    public void Interact()
    {
        Debug.Log($"Picked up {collectibleName}");
        Destroy(gameObject);
    }

    public string GetInteractionPrompt()
    {
        return $"Pick up {collectibleName}";
    }
}
