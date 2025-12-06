using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChase : MonoBehaviour
{
    public float moveSpeed = 2f;

    private Transform target;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Find the player by tag so you don't have to assign it manually
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("EnemyChase: No object with tag 'Player' found.");
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector2 currentPos = rb.position;
        Vector2 targetPos = target.position;
        Vector2 dir = (targetPos - currentPos).normalized;

        rb.MovePosition(currentPos + dir * moveSpeed * Time.fixedDeltaTime);
    }
}
