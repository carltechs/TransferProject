using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/NPC Dialogue")]
public class NPCDialogueData : ScriptableObject
{
    public string npcName = "NPC";
    public Sprite npcPortrait;
    [TextArea(3, 5)]
    public string[] dialogueLines = new string[] { "Hello!" };
    public float typingSpeed = 0.05f;
}