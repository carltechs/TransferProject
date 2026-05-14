using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Mobile Joystick")]
    public MobileJoystick joystick;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();    // <-- CHANGED
    }

    void Update()
    {
        if (joystick != null && joystick.isTouching)
        {
            moveDirection = joystick.inputDirection;
            Debug.Log("Joystick active - inputDirection: " + joystick.inputDirection + "  isTouching: " + joystick.isTouching);
        }
        else
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector2(moveX, moveY).normalized;
            // optional: Debug.Log("Keyboard moveDirection: " + moveDirection);
        }

        if (animator != null)
        {
            animator.SetFloat("moveX", moveDirection.x);
            animator.SetFloat("moveY", moveDirection.y);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveDirection * moveSpeed;
    }
}