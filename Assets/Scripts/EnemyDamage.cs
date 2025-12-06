using UnityEngine;

//yuhh

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        Health health = collision.collider.GetComponent<Health>();
        if (health == null) return;

        // Direction from enemy to player
        Vector2 dir = (collision.collider.transform.position - transform.position);
        health.TakeDamage(damage, dir);
    }

    // If you use triggers instead of collisions, use this instead:
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Health health = other.GetComponent<Health>();
        if (health == null) return;

        Vector2 dir = (other.transform.position - transform.position);
        health.TakeDamage(damage, dir);
    }
    */
}
