using UnityEngine;

//player combat system but melee doesnt work </3

public class PlayerCombat : MonoBehaviour
{
    public int meleeDamage = 1;
    public float meleeRange = 1f;
    public LayerMask enemyLayer;

    public Transform gunHolder;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float fireRate = 0.25f;
    private float nextShotTime;

    [Header("Animation")]
    [SerializeField] private string attackTrigger = "Attack";
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMelee();
        HandleGun();
    }

    void HandleMelee()
    {
    // Early-out if the key wasn't pressed this frame
    if (!Input.GetKeyDown(KeyCode.Space))
        return;

    // Trigger animation immediately so there's no delay
    PlayAttackAnimation();

    // Only then do the physics work
    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, meleeRange);

    for (int i = 0; i < hits.Length; i++)
        {
        Collider2D h = hits[i];

        // Skip anything that isn't an enemy
        if (!h.CompareTag("Enemy"))
            continue;

        // Optional: make sure we don't somehow hit ourselves
        if (h.gameObject == gameObject)
            continue;

        Health health = h.GetComponent<Health>();
        if (health == null)
            continue;

        health.TakeDamage(meleeDamage);
        }
    }

    void HandleGun()
    {
        if (!GameManager.Instance.hasGun) return;

        // aim
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - gunHolder.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        gunHolder.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // shoot
        if (Input.GetMouseButton(0) && Time.time >= nextShotTime)
        {
            nextShotTime = Time.time + fireRate;
            Shoot(dir.normalized);
        }
    }
    
    void Shoot(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, gunHolder.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletSpeed;
    }

    private void PlayAttackAnimation()
    {
        if (animator != null && !string.IsNullOrEmpty(attackTrigger))
        {
            animator.SetTrigger(attackTrigger);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
