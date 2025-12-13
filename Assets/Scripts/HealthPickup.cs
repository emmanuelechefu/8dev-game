using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Tooltip("Amount of health restored when the player collects this pickup.")]
    public int healAmount = 1;

    [Tooltip("Optional sound to play when the pickup is collected.")]
    public AudioClip pickupSound;

    // **CHANGE 1: Added volume control. Range slider makes it easy to edit in Inspector.**
    // **Functionality: Allows you to boost the volume (e.g., to 2.5) so it is louder than background music.**
    [Range(0f, 3f)]
    public float soundVolume = 2.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Health health = other.GetComponent<Health>();
        if (health == null) return;

        // **CHANGE 2: Added check for Max Health.**
        // **Functionality: Prevents the item from being destroyed/used if the player doesn't need it.**
        if (health.CurrentHealth >= health.MaxHealth)
        {
            return;
        }

        health.Heal(healAmount);

        if (pickupSound != null)
        {
           // ** CHANGE: Play sound at the CAMERA position, not the potion position. **
            // ** This prevents the sound from fading out due to distance. **
            Vector3 soundPos = Camera.main != null ? Camera.main.transform.position : transform.position;
            
            // Keep the Z at 0 just in case
            soundPos.z = 0f; 

            AudioSource.PlayClipAtPoint(pickupSound, soundPos, soundVolume);
        }

        Destroy(gameObject);
    }
}