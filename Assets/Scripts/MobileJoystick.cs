using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("UI References")]
    [SerializeField] private Image background;
    [SerializeField] private Image handle;

    [Header("Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float handleRange = 1f;
    [SerializeField] private float deadZone = 0.15f;

    private Vector2 inputVector = Vector2.zero;
    private bool touching;

    public bool isTouching => touching;
    public Vector2 inputDirection
    {
        get
        {
            if (inputVector.magnitude < deadZone)
                return Vector2.zero;
            return inputVector.normalized;
        }
    }

    private void Awake()
    {
        if (background == null)
            background = GetComponent<Image>();
        if (handle == null && transform.childCount > 0)
            handle = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touching = true;
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchPos = eventData.position;
        Vector2 direction = touchPos - (Vector2)background.rectTransform.position;
        float radius = (background.rectTransform.sizeDelta.x / 2f) * handleRange;

        inputVector = (direction.magnitude > radius) ? direction.normalized * radius : direction;
        handle.rectTransform.anchoredPosition = inputVector;
        inputVector /= radius;

        // No need to invoke OnValueChanged unless you use it elsewhere
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        touching = false;
        inputVector = Vector2.zero;
        handle.rectTransform.anchoredPosition = Vector2.zero;
    }
}