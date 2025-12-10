using UnityEngine;

// Simple door/portal that sends the player to the Open World scene
// when they enter the trigger.
public class OpenWorldDoor : MonoBehaviour
{
    private bool isLoading = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isLoading) return;
        if (!other.CompareTag("Player")) 
            return;

        isLoading = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadOpenWorld();
        }     
        else
        {
            Debug.LogWarning("GameManager instance not found. Cannot load Open World scene.");
        }
    }
}