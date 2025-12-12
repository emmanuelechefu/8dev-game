using UnityEngine;

public class OlapiumMerchant : MonoBehaviour
{
    [SerializeField] private ShopMenu shopMenu; 
    [SerializeField] private GameObject interactPrompt;
    
    // The shop E override code from before the errors is kept here
    private bool playerInRange = false;

    private void Start()
    {
        if (interactPrompt != null) interactPrompt.SetActive(false);
    }

    private void Update()
    {
        // OPEN SHOP
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // FIX: Use shopMenu.OpenShop() to call the function
            if (shopMenu != null) shopMenu.OpenShop();
        }

        // CLOSE SHOP 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // FIX: Use shopMenu.CloseShop() to call the function
            if (shopMenu != null) shopMenu.CloseShop();
        }
    } 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactPrompt != null) interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; 
            if (shopMenu != null) shopMenu.CloseShop(); 
            if (interactPrompt != null) interactPrompt.SetActive(false);
        }
    }
    public static bool IsPlayerInRangeOfAnyMerchant()
    {
        // Find the one active OlapiumMerchant in the scene
        OlapiumMerchant merchant = FindFirstObjectByType<OlapiumMerchant>();
        return merchant != null && merchant.playerInRange;
    }
    
}