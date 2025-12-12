using UnityEngine;
using System.Collections; // ADD THIS BACK for feedback coroutines
using TMPro;             // ADD THIS BACK for feedback text

public enum ItemType { None, Matcha, AuraCookie }

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public int matchaCount = 0;
    public int auraCookieCount = 0;

    public ItemType currentItem = ItemType.None;
    
    // Kept the feedback fields to avoid losing links
    [Header("Consumption Feedback")]
    [SerializeField] private TMP_Text feedbackText; 
    [SerializeField] private float feedbackDisplayTime = 3f;
    private Coroutine feedbackCoroutine;

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

    private void Update()
    {
        // Select Item
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentItem = ItemType.Matcha;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentItem = ItemType.AuraCookie;

        // Use Item (No E key override check yet)
        if (Input.GetKeyDown(KeyCode.E)) ConsumeCurrentItem();
    }

    public void AddItem(ItemType item)
    {
        if (item == ItemType.Matcha) matchaCount++;
        if (item == ItemType.AuraCookie) auraCookieCount++;
    }

    private void ConsumeCurrentItem()
    {
        if (currentItem == ItemType.Matcha)
        {
            if (matchaCount > 0)
            {
                matchaCount--;
                GameManager.Instance.HealPlayer(999); 
                ShowFeedback("Drank Matcha! Full health restored.");
            }
        }
        else if (currentItem == ItemType.AuraCookie)
                {
                    if (auraCookieCount > 0)
                    {
                        auraCookieCount--;
                        
                        // --- ACTIVATE SPEED BOOST ---
                        GameObject player = GameObject.FindGameObjectWithTag("Player");
                        PlayerController2D playerController = player != null ? player.GetComponent<PlayerController2D>() : null;

                        if (playerController != null)
                        {
                            playerController.ActivateSpeedBoost(); // Calls the new function
                            ShowFeedback("Ate Dubai Chocolate! Speed boost active."); 
                        }
                        else
                        {
                            Debug.LogWarning("PlayerController2D not found on Player for speed boost!");
                        }
                    }
                }
    }
    
    // Kept the feedback functions
    private void ShowFeedback(string message)
    {
        if (feedbackText != null)
        {
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