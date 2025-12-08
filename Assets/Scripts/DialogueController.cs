using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }

    public GameObject panel;
    public Text text;
    public KeyCode advanceKey = KeyCode.Space;

    public PlayerController2D playerController;
    public PlayerCombat playerCombat;

    public bool clearEnemyBulletsOnStart = true;

    public bool IsActive { get; private set; }

    [Header("Audio (optional)")]
    public AudioSource sfxSource;
    public AudioClip advanceClip;
    [Range(0f, 2f)] public float advanceVolume = 1f;

    [Header("Background music (optional)")]
    public BossMusicController bossMusicController;
    public bool pauseMusicDuringDialogue = true;

    private void Awake()
    {
        Instance = this;

        if (panel != null)
            panel.SetActive(false);

        IsActive = false;
    }

    public Coroutine PlayDialogue(string[] lines, MonoBehaviour[] behavioursToPause = null)
    {
        return StartCoroutine(PlayDialogueRoutine(lines, behavioursToPause));
    }

    public IEnumerator PlayDialogueRoutine(string[] lines, MonoBehaviour[] behavioursToPause = null)
    {
        if (lines == null || lines.Length == 0 || panel == null || text == null)
            yield break;

        // Disable player controls
        if (playerController != null)
            playerController.enabled = false;

        if (playerCombat != null)
            playerCombat.enabled = false;

        // Disable extra behaviours (e.g. EnemyShooter, BossMovement)
        if (behavioursToPause != null)
        {
            foreach (var b in behavioursToPause)
            {
                if (b != null)
                    b.enabled = false;
            }
        }

        // Zero out velocities so nobody keeps drifting
        if (playerController != null)
        {
            var rb = playerController.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
        }

        if (behavioursToPause != null)
        {
            foreach (var b in behavioursToPause)
            {
                if (b == null) continue;
                var rb = b.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.linearVelocity = Vector2.zero;
            }
        }

        // Optionally clear any existing enemy bullets already on screen
        if (clearEnemyBulletsOnStart)
        {
            var bullets = Object.FindObjectsOfType<EnemyBullet>();
            foreach (var b in bullets)
            {
                if (b != null)
                    Object.Destroy(b.gameObject);
            }
        }

        // Pause background boss music while dialogue is active
        if (pauseMusicDuringDialogue && bossMusicController != null && bossMusicController.musicSource != null)
        {
            var src = bossMusicController.musicSource;
            if (src != null && src.isPlaying)
            {
                src.Pause();
            }
        }

        panel.SetActive(true);
        IsActive = true;

        // Play the advance/transition clip once at the start of this dialogue
        if (sfxSource != null && advanceClip != null)
        {
            sfxSource.PlayOneShot(advanceClip, advanceVolume);
        }

        for (int i = 0; i < lines.Length; i++)
        {
            text.text = lines[i];

            bool advanced = false;
            while (!advanced)
            {
                if (Input.GetKeyDown(advanceKey) || Input.GetKeyDown(KeyCode.Return))
                {
                    advanced = true;
                }
                yield return null;
            }
        }

        panel.SetActive(false);
        IsActive = false;

        // Resume background boss music when dialogue ends
        if (pauseMusicDuringDialogue && bossMusicController != null && bossMusicController.musicSource != null)
        {
            var src = bossMusicController.musicSource;
            if (src != null)
            {
                src.UnPause();
            }
        }

        // Re-enable behaviours
        if (behavioursToPause != null)
        {
            foreach (var b in behavioursToPause)
            {
                if (b != null)
                    b.enabled = true;
            }
        }

        if (playerController != null)
            playerController.enabled = true;

        if (playerCombat != null)
            playerCombat.enabled = true;
    }
}
