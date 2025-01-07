using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue System/Dialogue Node", fileName = "Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    public List<DialogueLine> lines;
    public List<DialogueChoice> choices;
}
