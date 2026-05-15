using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float bulletsPerSecond = 1f;
    [SerializeField] private int bulletDamage = 1;

    [Header("Upgrade (auto from SkillTreeManager)")]
    private float damageMultiplier = 1f;

    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    private Transform target;
    private float timeUntilFire;

    void Start()
    {
        if (SkillTreeManager.Instance != null)
            damageMultiplier = SkillTreeManager.Instance.totalDamageMultiplier;
    }

    void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / bulletsPerSecond)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);
        if (hits.Length > 0) target = hits[0].transform;
    }

    bool CheckTargetIsInRange() =>
        Vector2.Distance(target.position, transform.position) <= targetingRange;

    void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, 200f * Time.deltaTime);
    }

    void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.SetTarget(target);
        bullet.damageMultiplier = damageMultiplier;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }
}