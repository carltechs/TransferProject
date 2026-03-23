using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    private void Start()
    {
        // 1. Auto-assign Animator if missing
        if (anim == null) anim = GetComponent<Animator>();

        // 2. Safety Check: Make sure the path exists before starting
        if (LevelManager.main.path != null && LevelManager.main.path.Length > 0)
        {
            target = LevelManager.main.path[pathIndex];
        }
    }

    private void Update()
    {
        if (target == null) return;

        // Check if we reached the waypoint
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex >= LevelManager.main.path.Length)
            {
                // CRITICAL: Tell the Spawner we are gone before destroying
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }

        // 3. Smooth Sprite Flipping
        FlipSprite();
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        // Move towards the target
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    private void FlipSprite()
    {
        // Using rb.velocity is smart, but we check for a small threshold 
        // to prevent "jittering" when the enemy stops.
        if (rb.velocity.x < -0.05f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (rb.velocity.x > 0.05f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}