using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    //Variables that can be changed to what ever value we need them too so we don't have to change 10 different lines of code.
    private float horizontal;
    private bool sprinting;
    private float speed = 6f;
    private float crouchspeed = 3f;
    private float speedmin = 6f;
    private float speedmax = 12f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    public SpriteRenderer sc;
    public Animator animator;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        // Checks for inputs and returns -1 0 or 1
        horizontal = Input.GetAxisRaw("Horizontal");
        sprinting = Input.GetKey(KeyCode.LeftShift);

        //Makes the waslikng animation activate
        animator.SetFloat("speed", Mathf.Abs(horizontal));

        //This makes the player jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        //This make the jumping animation turn on
        if (rb.velocity.y > 0f)
        {
            animator.SetBool("isjumping", true);
        }
        //This one makes the jumping animation turn off
        if (rb.velocity.y == 0f)
        {
            animator.SetBool("isjumping", false);
        }
        //This Makes it so the more you press space the more you'll jump
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        LooseMomentum();
        Crouchingis();
        Sprint();
        Flip();
    }

    private void FixedUpdate()
    {
        // Movement speed for player for air
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        // Check If the player is grounded
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    private void Flip()
    {
        // Character Flip left and right
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void Sprint() 
    {
        if (sprinting == true)
        {
            // This makes the player run
            speed = speed + 0.1f;
            SpeedLimit();
        }
        if (sprinting && Input.GetKey(KeyCode.C) == false)
        {
            // This decreases the speed of the player to the normal speed.
            speed = speed - 0.0001f;
            if (speed <= speedmin)
            {
                speed = speedmin;
            }
        }
    }
    private void Crouchingis()
    {
        if (Input.GetKey(KeyCode.C) == true && IsGrounded())
        {
            // This makes the player crouch
            // !!! IMPORTANT : HERE WE NEED TO ADJUST BOX COLIDER WHILE CROUCHING
            animator.SetBool("iscrouching", true);
            speed = crouchspeed;
            if (Input.GetKey(KeyCode.LeftShift)){
                speed = crouchspeed;
            }
            
        }
        else
        {
            //This make the crouhing animation stop
            animator.SetBool("iscrouching", false);
        }
    }
    private void SpeedLimit() {
        //This limits the speed of the player so they don't go off flying like Uzain bolt.
        if (speed >= speedmax)
        {
            speed = speedmax;
        }
    }
    private void LooseMomentum()
    {
        //This make the player loose momentum when their not moving. It's for when their not moving and pressing shift they don't keep their max speed they revert back to normal speed slowly.
        if (horizontal == 0)
        {
            speed = speedmin;
        }
    }
    private IEnumerator DelayS()
    {
        //In case you ever need a delay Function.
        yield return new WaitForSeconds(1);
    }
}