using UnityEngine;

//The door in the safeArea, the tile is "2" I think check the levelmanager or whereever I defined all the tiles I forgor

public class LevelDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered door, loading level...");
            GameManager.Instance.LoadLevel();
        }
    }
}
