using UnityEngine;
using UnityEngine.SceneManagement;

//The main backbone of everything, has resouce system that is meant for workbench but it's kinda cooked

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int gold = 20;
    public int rubies;
    public int diamonds;
    public int keysCollected;
    public int maxKeys = 5;

    public bool hasGun;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddResource(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Gold: gold += amount; break;
            case ResourceType.Ruby: rubies += amount; break;
            case ResourceType.Diamond: diamonds += amount; break;
        }
    }

    public bool SpendGold(int amount)
    {
        if (gold < amount) return false;
        gold -= amount;
        return true;
    }

    public void AddKey()
    {
        keysCollected++;
        if (keysCollected >= maxKeys)
        {
            // unlock boss portal / button
            Debug.Log("Boss unlocked!");
        }
    }

    public void PlayerDied()
    {
        // Lose all progress
        gold = rubies = diamonds = keysCollected = 0;
        hasGun = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSafeArea() => SceneManager.LoadScene("SafeArea");
    public void LoadLevel() => SceneManager.LoadScene("Level");
    public void LoadBoss() => SceneManager.LoadScene("Boss");
    public void LoadOpenWorld() => SceneManager.LoadScene("OpenWorld");
    public void LoadBulletHellTest() => SceneManager.LoadScene("BulletHellTest");
}

public enum ResourceType { Gold, Ruby, Diamond }
