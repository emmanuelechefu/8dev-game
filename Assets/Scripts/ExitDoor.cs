using UnityEngine;

//the door to leave the level/dungeon, I think it's tile is "3"

public class ExitDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance.keysCollected > 0)
        {
            Debug.Log("Player has a key – exiting to SafeArea");
            GameManager.Instance.LoadSafeArea();
        }
        else
        {
            Debug.Log("You need a key to use this door!");
        }
    }
}
