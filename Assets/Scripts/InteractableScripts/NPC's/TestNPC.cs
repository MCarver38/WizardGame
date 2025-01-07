using UnityEngine;

public class TestNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcName;
    [SerializeField] private DialogueNode startingNode;
    [SerializeField] private DialogueManager dialogueManager;
    
    public void Interact()
    {
        TriggerDialogue(dialogueManager);
    }

    public string GetInteractionPrompt()
    {
        return $"Talk to {npcName}";
    }

    public void TriggerDialogue(DialogueManager dialogueManager)
    {
        if (startingNode != null)
        {
            dialogueManager.StartDialogue(startingNode);
        }
    }
}
