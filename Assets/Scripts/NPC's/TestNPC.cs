using UnityEngine;

public class TestNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcName;
    public void Interact()
    {
        Debug.Log($"Talking to {npcName}");
    }

    public string GetInteractionPrompt()
    {
        return $"Talk to {npcName}";
    }
}
