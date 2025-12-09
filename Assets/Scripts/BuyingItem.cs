using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyingItem : MonoBehaviour, IPointerDownHandler
{
    public bool InRect = false;
    public bool Pressed = false;
    public int ItemID;
    [SerializeField] private ShopMenu ShopMenu;

    public void OnPointerDown(PointerEventData data)
    {
        ShopMenu.BuyingMenu(ItemID);
    }
}
