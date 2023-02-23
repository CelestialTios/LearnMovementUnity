using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Parameters")]
    public float moveSpeed = 10f;
    public float fallGravityMod = 2f;
    public float smallJumpGravityMod = 2.75f;

    Vector2 move;
    Rigidbody2D rb;
    Animator animator;
    
    [Header("Jump")]
    public float jumpHeight = 10f;
    public bool jumping;

    [Header("Ground")]
    public bool grounded;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Comme pour le rigidbody, on met l'Animator en cache :
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (jumping && grounded)
        {
            Jump();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float xMove = Input.GetAxis("Horizontal") * moveSpeed;
        jumping = Input.GetButtonDown("Jump");
        move = Vector2.right * xMove;

        if (!grounded)
        {
            //Descente : 
            if (move.y < 0f)
            {
                move.y += Physics2D.gravity.y * rb.gravityScale * (fallGravityMod - 1f) * Time.deltaTime;
            }
            //On monte et la touche est relachée : la gravité est plus forte (on monte donc moins haut)
            else if ( move.y > 0f && !Input.GetButtonDown("Jump") )
            {
                move.y += Physics2D.gravity.y * rb.gravityScale * (smallJumpGravityMod - 1f) * Time.deltaTime;
                jumping = false;
            }
        }
        move.y = rb.velocity.y;

        rb.velocity = move;
    }

    public void Jump()
    {
        float jumpImpulse = Mathf.Sqrt(-2f * Physics2D.gravity.y * rb.gravityScale * jumpHeight);
        rb.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
        //Puis au moment du saut :
        animator.SetTrigger("Jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        var res = Vector2.Dot(contact.normal, Vector2.up);
        if (res == -1) animator.SetTrigger("StopJump");
        if (res == 1) grounded = true;

    }

}
