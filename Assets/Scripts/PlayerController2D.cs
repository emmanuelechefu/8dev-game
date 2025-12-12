using UnityEngine;
using System.Collections; // Added for Coroutines
using TMPro;              // Added for UI Text
using UnityEngine.UI;     // Added for UI Image (HUD)

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int facing = 1;

    // --- NEW FIELDS FOR AURA MODE ---
    private float originalMoveSpeed;
    private bool isSpeedBoosted = false;

    [Header("Aura Mode UI")]
    public TMP_Text auraTimerText; // Link the new timer text here
    public Image auraHudImage;     // Link the new HUD image here
    public float auraDuration = 60f; // 1 minute duration

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalMoveSpeed = moveSpeed; // Store initial speed
    }

    private void Start()
    {
        // Hide Aura UI at start
        if (auraTimerText != null) auraTimerText.gameObject.SetActive(false);
        if (auraHudImage != null) auraHudImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        // WASD / arrow keys
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x > 0 && transform.localScale.x < 0 || x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }

        anim.SetFloat("x", Mathf.Abs(x));
        anim.SetFloat("y", Mathf.Abs(y));

        moveInput = new Vector2(x, y).normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void Flip()
    {
        facing *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // --- NEW PUBLIC AURA MODE LOGIC ---
    public void ActivateSpeedBoost()
    {
        // If already boosting, reset the timer by stopping the old routine
        if (isSpeedBoosted)
        {
            StopAllCoroutines();
        }
        
        StartCoroutine(SpeedBoostRoutine());
    }

    private IEnumerator SpeedBoostRoutine()
    {
        isSpeedBoosted = true;
        moveSpeed = originalMoveSpeed * 2f; // DOUBLE THE SPEED!

        // Activate UI
        if (auraTimerText != null) auraTimerText.gameObject.SetActive(true);
        if (auraHudImage != null) auraHudImage.gameObject.SetActive(true);
        
        // (Music logic removed as requested)

        float timer = auraDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            
            // Update Timer UI (rounded time)
            if (auraTimerText != null)
            {
                auraTimerText.text = Mathf.CeilToInt(timer).ToString(); 
            }

            yield return null;
        }

        // --- Duration Ended ---
        isSpeedBoosted = false;
        moveSpeed = originalMoveSpeed; // Reset Speed

        // Reset UI
        if (auraTimerText != null) auraTimerText.gameObject.SetActive(false);
        if (auraHudImage != null) auraHudImage.gameObject.SetActive(false);
        

    }
}