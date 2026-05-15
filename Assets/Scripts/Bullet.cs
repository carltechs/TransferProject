using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int baseDamage = 1;
    public float damageMultiplier = 1f;
    private Transform target;

    public void SetTarget(Transform t) => target = t;

    void OnCollisionEnter2D(Collision2D col)
    {
        int finalDamage = Mathf.RoundToInt(baseDamage * damageMultiplier);
        col.gameObject.GetComponent<Health>()?.TakeDamage(finalDamage);
        Destroy(gameObject);
    }
}