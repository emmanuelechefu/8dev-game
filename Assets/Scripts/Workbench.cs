using UnityEngine;


//workbench to buy/craft weapons

public class Workbench : MonoBehaviour
{
    public int gunGoldCost = 3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // very rough: open a UI or just auto-buy for now
        TryBuyGun();
    }

    public void TryBuyGun()
    {
        if (GameManager.Instance.hasGun)
        {
            Debug.Log("Already have gun");
            return;
        }

        if (GameManager.Instance.SpendGold(gunGoldCost))
        {
            GameManager.Instance.hasGun = true;
            Debug.Log("Gun crafted!");
        }
        else
        {
            Debug.Log("Not enough gold");
        }
    }
}
