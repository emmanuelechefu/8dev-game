using UnityEngine;

public class OpenWorldPlayer : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D body;
    private Animator anim;
    private bool airborne;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

       body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.linearVelocity.y); 

        //Flip player depending on movement :P
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1,1,1);

       if(Input.GetKey(KeyCode.W) && !airborne)
       {
           Jump();
       }

       anim.SetBool("moving", horizontalInput != 0);
       anim.SetBool("airborne", airborne);
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, speed);
        anim.SetTrigger("jump");
        airborne = true;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            airborne = false;
        }
    }
}
