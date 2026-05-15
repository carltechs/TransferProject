using UnityEngine;

public class UpgraderNPC : MonoBehaviour, IInteractable
{
    public SkillTreeUI skillTreeUI;

    public bool CanInteract() => true;

    public void Interact()
    {
        if (skillTreeUI != null)
            skillTreeUI.Open();
        else
            Debug.LogWarning("SkillTreeUI not assigned to UpgraderNPC");
    }
}
