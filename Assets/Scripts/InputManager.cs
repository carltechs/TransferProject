using UnityEngine;
using UnityEngine.UI;  // Important for Button

public class InputManager : MonoBehaviour
{
    private InteractionDetector interactionDetector;
    public Button interactButton;  // This creates the field

    void Start()
    {
        interactionDetector = GetComponent<InteractionDetector>();

        if (interactButton != null)
        {
            interactButton.onClick.AddListener(() => {
                if (interactionDetector != null)
                    interactionDetector.TryInteract();
            });
            Debug.Log("Button connected!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interactionDetector != null)
                interactionDetector.TryInteract();
        }
    }
}