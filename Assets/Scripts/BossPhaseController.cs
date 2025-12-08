using UnityEngine;

public class BossPhaseController : MonoBehaviour
{
    public EnemyShooter shooter;
    public Health health;

    [Header("Phase thresholds (health ratio)")]
    [Range(0f, 1f)] public float phase2Threshold = 0.66f; // between phase 1 and 2
    [Range(0f, 1f)] public float phase3Threshold = 0.33f; // between phase 2 and 3

    [Header("Phase 1 (high health)")]
    public float phase1FireRate = 1.0f;
    public int phase1BulletsPerShot = 1;
    public float phase1SpreadAngle = 0f;
    public float phase1BulletSpeed = 6f;

    [Header("Phase 2 (mid health)")]
    public float phase2FireRate = 0.7f;
    public int phase2BulletsPerShot = 3;
    public float phase2SpreadAngle = 30f;
    public float phase2BulletSpeed = 7f;

    [Header("Phase 3 (low health)")]
    public float phase3FireRate = 0.4f;
    public int phase3BulletsPerShot = 8;
    public float phase3SpreadAngle = 90f;
    public float phase3BulletSpeed = 8f;

    [Header("Bullet prefabs per phase (optional)")]
    public GameObject phase1BulletPrefab;
    public GameObject phase2BulletPrefab;
    public GameObject phase3BulletPrefab;

    [Header("Dialogue per phase (optional)")]
    [TextArea] public string[] phase2Lines;
    [TextArea] public string[] phase3Lines;

    [Header("Transition audio per phase (optional)")]
    public AudioClip phase2TransitionClip;
    public AudioClip phase3TransitionClip;
    [Range(0f, 2f)] public float phase2TransitionVolume = 1f;
    [Range(0f, 2f)] public float phase3TransitionVolume = 1f;

    public BossMovement movement;
    public BossMusicController musicController;

    private int currentPhase = -1;

    private void Awake()
    {
        // Auto-find components even if they live on a child object of the boss prefab
        if (shooter == null)
            shooter = GetComponentInChildren<EnemyShooter>();

        if (health == null)
            health = GetComponentInChildren<Health>();

        if (movement == null)
            movement = GetComponentInChildren<BossMovement>();

        if (musicController == null)
            musicController = FindFirstObjectByType<BossMusicController>();
    }

    private void Start()
    {
        UpdatePhase(force: true);
    }

    private void Update()
    {
        UpdatePhase(force: false);
    }

    private void UpdatePhase(bool force)
    {
        if (shooter == null || health == null || health.MaxHealth <= 0)
            return;

        float ratio = (float)health.CurrentHealth / health.MaxHealth;

        int phase;
        if (ratio > phase2Threshold)
        {
            phase = 1;
        }
        else if (ratio > phase3Threshold)
        {
            phase = 2;
        }
        else
        {
            phase = 3;
        }

        if (!force && phase == currentPhase)
            return;

        currentPhase = phase;
        ApplyPhase(phase);
    }

    private void ApplyPhase(int phase)
    {
        DialogueController dialogue = DialogueController.Instance;

        switch (phase)
        {
            case 1:
                shooter.fireRate = phase1FireRate;
                shooter.bulletsPerShot = phase1BulletsPerShot;
                shooter.spreadAngle = phase1SpreadAngle;
                shooter.bulletSpeed = phase1BulletSpeed;
                if (phase1BulletPrefab != null)
                    shooter.bulletPrefab = phase1BulletPrefab;

                if (musicController != null)
                    musicController.PlayPhaseMusic(1);
                break;
            case 2:
                shooter.fireRate = phase2FireRate;
                shooter.bulletsPerShot = phase2BulletsPerShot;
                shooter.spreadAngle = phase2SpreadAngle;
                shooter.bulletSpeed = phase2BulletSpeed;
                if (phase2BulletPrefab != null)
                    shooter.bulletPrefab = phase2BulletPrefab;

                if (musicController != null)
                    musicController.PlayPhaseMusic(2);

                if (dialogue != null && phase2Lines != null && phase2Lines.Length > 0)
                {
                    if (phase2TransitionClip != null)
                    {
                        dialogue.advanceClip = phase2TransitionClip;
                        dialogue.advanceVolume = phase2TransitionVolume;
                    }

                    MonoBehaviour[] toPause = movement != null
                        ? new MonoBehaviour[] { shooter, movement }
                        : new MonoBehaviour[] { shooter };

                    dialogue.PlayDialogue(phase2Lines, toPause);
                }
                break;
            case 3:
                shooter.fireRate = phase3FireRate;
                shooter.bulletsPerShot = phase3BulletsPerShot;
                shooter.spreadAngle = phase3SpreadAngle;
                shooter.bulletSpeed = phase3BulletSpeed;
                if (phase3BulletPrefab != null)
                    shooter.bulletPrefab = phase3BulletPrefab;

                if (musicController != null)
                    musicController.PlayPhaseMusic(3);

                if (dialogue != null && phase3Lines != null && phase3Lines.Length > 0)
                {
                    if (phase3TransitionClip != null)
                    {
                        dialogue.advanceClip = phase3TransitionClip;
                        dialogue.advanceVolume = phase3TransitionVolume;
                    }

                    MonoBehaviour[] toPause = movement != null
                        ? new MonoBehaviour[] { shooter, movement }
                        : new MonoBehaviour[] { shooter };

                    dialogue.PlayDialogue(phase3Lines, toPause);
                }
                break;
        }
    }
}
