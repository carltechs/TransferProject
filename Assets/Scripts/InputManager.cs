using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InteractionDetector interactionDetector;

    void Start()
    {
        interactionDetector = GetComponent<InteractionDetector>();

        if (interactionDetector == null)
        {
            Debug.LogError("InteractionDetector not found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interactionDetector != null)
            {
                interactionDetector.TryInteract();
            }
        }
    }
}