using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel;
    

    public void Start()
    {
        shopPanel.SetActive(false);
    }
    public void OpenShop()
    {
        shopPanel.SetActive(true);
        
    }

    public void BuyingMenu(int ItemID)
    {
        Debug.Log("Menu opened");

        if (GameManager.Instance.hasGun)
        {
            Debug.Log("Already have gun");
            return;
        }

        //buying bullet type based off of which weapon clicked, need proper implementation
        switch(ItemID)
        {
            case 0:
                Debug.Log("bought matcha bullets");
                break;
            case 1:
                Debug.Log("bought tekken bullets");
                break;
            case 2:
                Debug.Log("bought ghost bullets");
                break;
        }

    }


    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }
}
