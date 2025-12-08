using UnityEngine;

// Simple door/portal that sends the player to the Boss scene
// when they enter the trigger.
public class BossDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadBoss();
        }
    }
}
