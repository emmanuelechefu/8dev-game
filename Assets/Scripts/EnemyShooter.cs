using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 1.2f;
    public float initialDelay = 0.5f;
    public float bulletSpeed = 6f;

    public bool aimAtPlayer = true;
    public int bulletsPerShot = 1;
    public float spreadAngle = 0f;

    [Header("Randomization (optional)")]
    public bool useRandomFireInterval = false;
    public float minFireInterval = 0.3f;
    public float maxFireInterval = 1.2f;

    public bool useRandomBulletsPerShot = false;
    public int minBulletsPerShot = 1;
    public int maxBulletsPerShot = 5;

    public bool useRandomSpreadAngle = false;
    public float minSpreadAngle = 0f;
    public float maxSpreadAngle = 90f;

    private Transform target;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }

        if (bulletPrefab != null)
        {
            StartCoroutine(ShootLoop());
        }
    }

    private IEnumerator ShootLoop()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            Fire();

            float wait = fireRate;
            if (useRandomFireInterval)
            {
                float min = Mathf.Max(0.05f, minFireInterval);
                float max = Mathf.Max(min + 0.01f, maxFireInterval);
                wait = Random.Range(min, max);
            }

            yield return new WaitForSeconds(wait);
        }
    }

    private void Fire()
    {
        if (bulletPrefab == null)
        {
            return;
        }

        // Don't fire while any dialogue panel is active
        if (DialogueController.Instance != null && DialogueController.Instance.IsActive)
        {
            return;
        }

        if (aimAtPlayer && (target == null || !target.gameObject.activeInHierarchy))
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
        }

        Vector2 baseDir;

        if (aimAtPlayer && target != null)
        {
            baseDir = (target.position - transform.position).normalized;
        }
        else
        {
            baseDir = transform.right;
        }

        int bullets = bulletsPerShot;
        float spread = spreadAngle;

        if (useRandomBulletsPerShot)
        {
            int min = Mathf.Max(1, minBulletsPerShot);
            int max = Mathf.Max(min, maxBulletsPerShot);
            bullets = Random.Range(min, max + 1);
        }

        if (useRandomSpreadAngle)
        {
            float min = Mathf.Min(minSpreadAngle, maxSpreadAngle);
            float max = Mathf.Max(minSpreadAngle, maxSpreadAngle);
            spread = Random.Range(min, max);
        }

        if (bullets <= 1 || spread <= 0.01f)
        {
            SpawnBullet(baseDir);
            return;
        }

        if (bullets == 2)
        {
            float half = spread * 0.5f;
            SpawnBullet(Rotate(baseDir, -half));
            SpawnBullet(Rotate(baseDir, half));
            return;
        }

        for (int i = 0; i < bullets; i++)
        {
            float t = bullets == 1 ? 0f : i / (float)(bullets - 1);
            float angle = Mathf.Lerp(-spread * 0.5f, spread * 0.5f, t);
            Vector2 dir = Rotate(baseDir, angle);
            SpawnBullet(dir);
        }
    }

    private void SpawnBullet(Vector2 dir)
    {
        GameObject instance = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        EnemyBullet enemyBullet = instance.GetComponent<EnemyBullet>();
        if (enemyBullet != null)
        {
            enemyBullet.speed = bulletSpeed;
            enemyBullet.Initialize(dir);
        }
        else
        {
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = dir.normalized * bulletSpeed;
            }
        }
    }

    private static Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);

        float x = v.x * cos - v.y * sin;
        float y = v.x * sin + v.y * cos;

        return new Vector2(x, y).normalized;
    }
}
