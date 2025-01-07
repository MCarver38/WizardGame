using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private GameObject choiceContainer;
    [SerializeField] private Button choicePrefabButton;

    private DialogueNode currentNode;
    private int currentLineIndex;

    public void StartDialogue(DialogueNode startingNode)
    {
        currentNode = startingNode;
        currentLineIndex = 0;
        DisplayCurrentLine();
    }

    private void DisplayCurrentLine()
    {
        if (currentLineIndex < currentNode.lines.Count)
        {
            var line = currentNode.lines[currentLineIndex];
            speakerText.text = line.speaker;
            dialogueText.text = line.text;
            currentLineIndex++;
        }
        else
        {
            DisplayChoices();
        }
    }

    private void DisplayChoices()
    {
        foreach (Transform child in choiceContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var choice in currentNode.choices)
        {
            var button = Instantiate(choicePrefabButton, choiceContainer.transform);
            button.GetComponentInChildren<Text>().text = choice.choiceText;
            button.onClick.AddListener(() => OnChoiceSelected(choice.nextNode));
        }
    }

    private void OnChoiceSelected(DialogueNode nextNode)
    {
        if (nextNode != null)
        {
            StartDialogue(nextNode);
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialogueText.text = "";
        speakerText.text = "";
        foreach (Transform child in choiceContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentNode != null && currentLineIndex < currentNode.lines.Count)
        {
            DisplayCurrentLine();
        }
    }
}
