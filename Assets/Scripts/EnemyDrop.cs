using UnityEngine;

//I dont know if ts works lmaooo. When I try test I randomly get gold so idk what's up with that

public class EnemyDrop : MonoBehaviour
{
    public int goldDrop = 1;
    public int rubyDrop = 0;
    public int diamondDrop = 0;

    [Header("Health Drop")]
    [Range(0f, 1f)] public float healthDropChance = 0.25f;
    public int healthRestoreAmount = 1;
    public GameObject healthPickupPrefab;

    public void DropLoot()
    
    {
        // 1. Give Resources
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddResource(ResourceType.Gold, goldDrop);
            GameManager.Instance.AddResource(ResourceType.Ruby, rubyDrop);
            GameManager.Instance.AddResource(ResourceType.Diamond, diamondDrop);
        }

        if (healthPickupPrefab != null && Random.value <= healthDropChance)
        {
            GameObject pickup = Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);
            HealthPickup healthPickup = pickup.GetComponent<HealthPickup>();

            if (healthPickup != null && healthRestoreAmount > 0)
            {
                healthPickup.healAmount = healthRestoreAmount;
            }
        } /*hi baka change by me abel san*/
    }
}
