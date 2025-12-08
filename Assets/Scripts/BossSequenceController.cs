using System.Collections;
using UnityEngine;

public class BossSequenceController : MonoBehaviour
{
    public Transform bossSpawnPoint;
    public GameObject[] nextBossPrefabs;

    [Header("Dialogue between bosses (optional)")]
    [TextArea] public string[] betweenBossLines;
    public DialogueController dialogueController;
    public AudioClip betweenBossTransitionClip;
    [Range(0f, 2f)] public float betweenBossTransitionVolume = 1f;

    [Header("Music per boss (optional)")]
    public BossMusicController bossMusicController;
    public AudioClip[] bossMusicPerBoss;
    public float[] bossMusicVolumePerBoss;

    private GameObject currentBoss;
    private Health currentHealth;
    private int nextIndex = 0;
    private bool sequenceStarted = false;
    private bool isSpawningNext = false;
    private int currentBossIndex = -1;

    public void RegisterCurrentBoss(GameObject boss)
    {
        currentBoss = boss;
        currentHealth = boss != null ? boss.GetComponent<Health>() : null;
        sequenceStarted = true;

        // First boss in the sequence uses index 0 for music, if configured
        if (currentBossIndex < 0)
        {
            currentBossIndex = 0;
        }

        ApplyBossMusic(currentBossIndex);
    }

    private void Update()
    {
        if (!sequenceStarted)
            return;

        if (currentBoss == null || currentHealth == null || currentHealth.CurrentHealth <= 0)
        {
            TrySpawnNextBoss();
        }
    }

    private void TrySpawnNextBoss()
    {
        if (isSpawningNext)
            return;

        StartCoroutine(SpawnNextBossRoutine());
    }

    private IEnumerator SpawnNextBossRoutine()
    {
        isSpawningNext = true;

        if (dialogueController != null && betweenBossLines != null && betweenBossLines.Length > 0)
        {
            if (betweenBossTransitionClip != null)
            {
                dialogueController.advanceClip = betweenBossTransitionClip;
                dialogueController.advanceVolume = betweenBossTransitionVolume;
            }

            yield return dialogueController.PlayDialogueRoutine(betweenBossLines, null);
        }

        if (nextBossPrefabs == null || nextBossPrefabs.Length == 0)
        {
            isSpawningNext = false;
            yield break;
        }

        // No more bosses configured: sequence finished → go back to SafeArea
        if (nextIndex >= nextBossPrefabs.Length)
        {
            isSpawningNext = false;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadSafeArea();
            }

            yield break;
        }

        if (bossSpawnPoint == null)
        {
            isSpawningNext = false;
            yield break;
        }

        GameObject prefab = nextBossPrefabs[nextIndex];
        if (prefab == null)
        {
            nextIndex++;
            isSpawningNext = false;
            yield break;
        }

        currentBoss = Object.Instantiate(prefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
        currentHealth = currentBoss.GetComponent<Health>();

        // Advance to the next boss index for music (boss 1 = 0, boss 2 = 1, etc.)
        currentBossIndex++;
        ApplyBossMusic(currentBossIndex);

        nextIndex++;
        isSpawningNext = false;
    }

    private void ApplyBossMusic(int index)
    {
        if (bossMusicController == null)
            return;

        if (bossMusicPerBoss == null || index < 0 || index >= bossMusicPerBoss.Length)
            return;

        AudioClip clip = bossMusicPerBoss[index];
        if (clip == null)
            return;

        // Treat this as the overarching track for this boss.
        // We assign it as phase 1 music and clear others so
        // phase changes won't swap the BGM.
        bossMusicController.phase1Music = clip;
        bossMusicController.phase2Music = null;
        bossMusicController.phase3Music = null;

        // Apply per-boss volume if configured (value > 0), otherwise keep current volume
        if (bossMusicController.musicSource != null)
        {
            if (bossMusicVolumePerBoss != null && index >= 0 && index < bossMusicVolumePerBoss.Length)
            {
                float configured = bossMusicVolumePerBoss[index];
                if (configured > 0f)
                {
                    bossMusicController.musicSource.volume = configured;
                }
            }
        }

        bossMusicController.PlayPhaseMusic(1);
    }
}
