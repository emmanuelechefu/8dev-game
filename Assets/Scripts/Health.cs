using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

//HP System for Enemies/Player, keep isPlayer unticked if you're messing with a non player prefab in inspector

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public bool isPlayer;

    [Header("Player-only")]
    public float invulnerabilityTime = 1.5f;
    public float knockbackForce = 5f;

    public Slider slider;

    private int current;
    private bool isInvulnerable;
    private SpriteRenderer sr;
    private Color originalColor;

    public int CurrentHealth => current;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        current = maxHealth;

        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;

        if (slider != null)
        {
            slider.maxValue = maxHealth;
            slider.value = current;
        }
    }

    // Old calls (bullets, melee) still work:
    public void TakeDamage(int amount)
    {
        TakeDamage(amount, Vector2.zero);
    }

    // New overload: pass direction for knockback if desired
    public void TakeDamage(int amount, Vector2 knockbackDirection)
    {
        Console.WriteLine("Ow");
        if (isPlayer && isInvulnerable)
            return;

        current -= amount;
        if (slider != null)
        {
            slider.value = current;
        }

        if (isPlayer)
        {
            // Knockback
            if (knockbackDirection.sqrMagnitude > 0.0001f)
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    knockbackDirection.Normalize();
                    rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }
            }

            // Start invulnerability frames
            if (invulnerabilityTime > 0f)
                StartCoroutine(InvulnerabilityRoutine());
        }

        if (current <= 0)
        {
            if (isPlayer)
            {
                GameManager.Instance.PlayerDied();
            }

            Destroy(gameObject);
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        float elapsed = 0f;
        while (elapsed < invulnerabilityTime)
        {
            // Optional: flicker sprite to show invuln
            if (sr != null)
                sr.color = new Color(1f, 1f, 1f, 0.3f);

            yield return new WaitForSeconds(0.1f);

            if (sr != null)
                sr.color = originalColor;

            yield return new WaitForSeconds(0.1f);

            elapsed += 0.2f;
        }

        isInvulnerable = false;

        if (sr != null)
            sr.color = originalColor;
    }

public void Heal(int amount)
    {
        current += amount;
        if (current > maxHealth)
        {
            current = maxHealth;
        }

        if (slider != null) 
        {
            slider.value = current;
        }


        Debug.Log(gameObject.name + " healed. Current HP: " + current);
    }
}
