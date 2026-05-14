using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    [Header("Settings")]
    public float detectionRadius = 2f;
    public GameObject interactionIcon;

    private IInteractable currentInteractable = null;

    void Start()
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(false);
    }

    void Update()
    {
        DetectInteractable();
    }

    void DetectInteractable()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        IInteractable foundInteractable = null;

        foreach (Collider2D collider in hitColliders)
        {
            IInteractable interactable = collider.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract())
            {
                foundInteractable = interactable;
                break;
            }
        }

        if (foundInteractable != currentInteractable)
        {
            currentInteractable = foundInteractable;
            if (interactionIcon != null)
                interactionIcon.SetActive(currentInteractable != null);
        }
    }

    public void TryInteract()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}