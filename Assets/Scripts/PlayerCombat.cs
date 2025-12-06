using UnityEngine;

//player combat system but melee doesnt work </3

public class PlayerCombat : MonoBehaviour
{
    public int meleeDamage = 1;
    public float meleeRange = 0.6f;
    public LayerMask enemyLayer;

    public Transform gunHolder;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float fireRate = 0.25f;
    private float nextShotTime;

    private void Update()
    {
        HandleMelee();
        HandleGun();
    }

    void HandleMelee()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, meleeRange);

            foreach (var h in hits)
            {
                if (!h.CompareTag("Enemy")) continue;

                var health = h.GetComponent<Health>();
                if (health != null)
                    health.TakeDamage(meleeDamage);
            }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
