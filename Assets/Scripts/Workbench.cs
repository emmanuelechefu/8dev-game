using System;
using UnityEngine;


//workbench to buy/craft weapons

public class Workbench : MonoBehaviour
{
    [SerializeField] private ShopMenu ShopMenu;
    private KeyCode InteractKey = KeyCode.E;
    private bool PlayerInRange = false;

    private void Update()
    {
        if (PlayerInRange && Input.GetKeyDown(InteractKey))
        {
            // opens UI to buy/craft weapon
            ShopMenu.OpenShop();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ShopMenu.CloseShop();
        }
    } 


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;
        }
 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = false; 
            ShopMenu.CloseShop();
        }
    }
}
