using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    public NPCDialogueData dialogueData;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image portraitImage;
    public float autoAdvanceDelay = 2f;

    [Header("Scene Switching")]
    public string sceneToLoad = "TowerD 1";
    public bool switchSceneAfterDialogue = true;

    private int currentLine = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        currentLine = 0;

        nameText.text = dialogueData.npcName;
        portraitImage.sprite = dialogueData.npcPortrait;
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in dialogueData.dialogueLines[currentLine])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;
        yield return new WaitForSeconds(autoAdvanceDelay);
        NextLine();
    }

    private void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = dialogueData.dialogueLines[currentLine];
            isTyping = false;
        }
        else
        {
            currentLine++;
            if (currentLine < dialogueData.dialogueLines.Length)
            {
                StartCoroutine(TypeLine());
            }
            else
            {
                EndDialogue();
            }
        }
    }

    private void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        if (switchSceneAfterDialogue)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}