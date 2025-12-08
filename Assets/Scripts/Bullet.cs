using UnityEngine;

//Bullets are also cooked they are affected by grav but should be easy fix

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Hit enemy
        if (other.CompareTag("Enemy"))
        {
            var health = other.GetComponentInParent<Health>();
            if (health != null)
                health.TakeDamage(damage);

            Destroy(gameObject);
            return;
        }

        // Hit wall
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
