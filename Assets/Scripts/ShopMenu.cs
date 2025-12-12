using UnityEngine;
using System.Collections; 
using TMPro;


// This creates the Dropdown List options
public enum ShopItem 
{ 
    None, 
    TekkenWeapon, 
    GhostWeapon, 
    MatchaBullets, 
    MatchaDrink, 
    DubaiChocolate 
}

public class ShopMenu : MonoBehaviour
{

    [Header("Feedback")]
    [SerializeField] private TMP_Text feedbackText; // Drag a TextMeshPro component here
    [SerializeField] private float feedbackDisplayTime = 3f;

    private Coroutine feedbackCoroutine;
    [SerializeField] private GameObject shopPanel;

    [Header("Bullet Prefabs")]
    public GameObject tekkenBulletPrefab;  
    public GameObject ghostBulletPrefab;   
    public GameObject matchaBulletPrefab;  

    private void Start()
    {
        if (shopPanel != null) shopPanel.SetActive(false);
    }

    public void OpenShop() => shopPanel.SetActive(true);
    public void CloseShop() => shopPanel.SetActive(false);

    // Now accepts a Name instead of a Number
    public void BuyingMenu(ShopItem item)
    {
        PlayerCombat playerCombat = FindFirstObjectByType<PlayerCombat>();

        switch(item)
        {
            // --- WEAPONS ---
            case ShopItem.TekkenWeapon:
                if (GameManager.Instance.RemoveResource(ResourceType.Gold, 12))
                {
                    if (playerCombat != null) playerCombat.bulletPrefab = tekkenBulletPrefab;
                    Debug.Log("Bought Tekken Weapon");
                }
                break;

            case ShopItem.GhostWeapon:
                if (GameManager.Instance.RemoveResource(ResourceType.Gold, 9))
                {
                    if (playerCombat != null) playerCombat.bulletPrefab = ghostBulletPrefab;
                    Debug.Log("Bought Ghost Weapon");
                }
                break;

            case ShopItem.MatchaBullets:
                if (GameManager.Instance.RemoveResource(ResourceType.Gold, 9))
                {
                    if (playerCombat != null) playerCombat.bulletPrefab = matchaBulletPrefab;
                    Debug.Log("Bought Matcha Bullets");
                }
                break;

            // --- CONSUMABLES ---
            case ShopItem.MatchaDrink:
                if (GameManager.Instance.RemoveResource(ResourceType.Gold, 10))
                {
                    Inventory.Instance.AddItem(ItemType.Matcha);
                    Debug.Log("Bought Matcha Drink");
                }
                else
                {
                    ShowFeedback("Olapium: You need more Gold!"); // ADDED
                }
                break;

            case ShopItem.DubaiChocolate:
                if (GameManager.Instance.RemoveResource(ResourceType.Gold, 20))
                {
                    Inventory.Instance.AddItem(ItemType.AuraCookie);
                    Debug.Log("Bought Dubai Chocolate");
                }
                else
                {
                    ShowFeedback("Olapium: You need more Gold!"); // ADDED
                }
                break;
        }
    }

    private void ShowFeedback(string message)
    {
        if (feedbackText != null)
        {
            // Stop previous message if one is running
            if (feedbackCoroutine != null)
            {
                StopCoroutine(feedbackCoroutine);
            }
            feedbackCoroutine = StartCoroutine(ShowFeedbackRoutine(message));
        }
    }

    private IEnumerator ShowFeedbackRoutine(string message)
    {
        feedbackText.text = message;
        feedbackText.gameObject.SetActive(true);
        yield return new WaitForSeconds(feedbackDisplayTime);
        feedbackText.gameObject.SetActive(false);
        feedbackCoroutine = null;
    }
}