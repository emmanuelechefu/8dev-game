using UnityEngine;

//Code for key so when you have a key it's added

public class KeyPickup : MonoBehaviour
{
    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;                // already picked up
        if (!other.CompareTag("Player")) return;

        collected = true;

        // Use your GameManager's method if you have one
        GameManager.Instance.AddKey();
        // or: GameManager.Instance.keysCollected++;

        Debug.Log("Key collected. Keys = " + GameManager.Instance.keysCollected);

        Destroy(gameObject);
    }
}
