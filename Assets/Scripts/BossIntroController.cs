using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossIntroController : MonoBehaviour
{
    public GameObject introPanel;
    public Text introText;
    [TextArea]
    public string[] introLines;

    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public BossSequenceController bossSequence;

    public PlayerController2D playerController;
    public PlayerCombat playerCombat;

    public bool waitForKey = true;
    public KeyCode continueKey = KeyCode.Space;
    public float delayBeforeBossSpawn = 1f;

    private void Start()
    {
        if (introPanel != null)
            introPanel.SetActive(true);

        if (introText != null && introLines != null && introLines.Length > 0)
            introText.text = introLines[0];

        if (playerController != null)
            playerController.enabled = false;

        if (playerCombat != null)
            playerCombat.enabled = false;

        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        int index = 0;

        if (introLines != null && introLines.Length > 0)
        {
            while (index < introLines.Length)
            {
                if (introText != null)
                    introText.text = introLines[index];

                bool advanced = false;
                while (!advanced)
                {
                    if (Input.GetKeyDown(continueKey) || Input.GetKeyDown(KeyCode.Return))
                    {
                        advanced = true;
                    }
                    yield return null;
                }

                index++;
            }
        }

        if (introPanel != null)
            introPanel.SetActive(false);

        if (playerController != null)
            playerController.enabled = true;

        if (playerCombat != null)
            playerCombat.enabled = true;

        yield return new WaitForSeconds(delayBeforeBossSpawn);

        if (bossPrefab != null && bossSpawnPoint != null)
        {
            GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);

            if (bossSequence != null)
            {
                bossSequence.RegisterCurrentBoss(boss);
            }
        }
    }
}
