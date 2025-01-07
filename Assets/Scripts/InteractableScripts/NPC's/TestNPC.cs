using UnityEngine;

public class TestNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcName;
    [SerializeField] private DialogueNode startingNode;
    public void Interact()
    {
        Debug.Log($"Talking to {npcName}");
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
