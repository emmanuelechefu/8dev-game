using UnityEngine;
using UnityEngine.EventSystems;

public class BuyingItem : MonoBehaviour, IPointerDownHandler
{
    // This creates the Dropdown in the Inspector!
    public ShopItem itemToBuy; 
    
    [SerializeField] private ShopMenu shopMenu;

    public void OnPointerDown(PointerEventData data)
    {
        if (shopMenu != null)
        {
            shopMenu.BuyingMenu(itemToBuy);
        }
    }
}