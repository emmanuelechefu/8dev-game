using UnityEngine;

// Door/portal that sends the player to the BulletHellTest scene
// when they enter the trigger.
public class BulletHellDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadBulletHellTest();
        }
    }
}
