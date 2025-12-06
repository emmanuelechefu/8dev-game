using UnityEditor.Tilemaps;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int facing = 1;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

}
