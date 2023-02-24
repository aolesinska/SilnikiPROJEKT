using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 15f;

    private enum MovementState { idle, run, jump, fall, crouch }

    [SerializeField] private AudioSource jumpSoundEffect;
    
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {        
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        
        
        UpdateAnimationState();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            anim.SetBool("crouch", true);
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
        else
        {
            anim.SetBool("crouch", false);
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }        
    }

    public void Move(InputAction.CallbackContext context) 
    {
        dirX = context.ReadValue<Vector2>().x;
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0f)
        {
            state = MovementState.run;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.run;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jump;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.fall;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
