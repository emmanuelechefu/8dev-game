using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("Slot 1: Matcha")]
    public Image slot1Icon;
    public GameObject slot1Border; 
    public TMP_Text slot1Count;

    [Header("Slot 2: Dubai Chocolate")]
    public Image slot2Icon;
    public GameObject slot2Border;
    public TMP_Text slot2Count;

    [Header("Art Assets")]
    public Sprite matchaSprite;
    public Sprite cookieSprite;

    private void Start()
    {
        if(slot1Icon != null) slot1Icon.sprite = matchaSprite;
        if(slot2Icon != null) slot2Icon.sprite = cookieSprite;
    }

    private void Update()
    {
        if (Inventory.Instance == null) return;

        // Update Numbers
        if(slot1Count != null) slot1Count.text = Inventory.Instance.matchaCount.ToString();
        if(slot2Count != null) slot2Count.text = Inventory.Instance.auraCookieCount.ToString();

        // Update Selection Border
        ItemType current = Inventory.Instance.currentItem;

        if (current == ItemType.Matcha)
        {
            if(slot1Border) slot1Border.SetActive(true);
            if(slot2Border) slot2Border.SetActive(false);
        }
        else if (current == ItemType.AuraCookie)
        {
            if(slot1Border) slot1Border.SetActive(false);
            if(slot2Border) slot2Border.SetActive(true);
        }
        else
        {
            if(slot1Border) slot1Border.SetActive(false);
            if(slot2Border) slot2Border.SetActive(false);
        }
    }
}