using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    private float horizontal;
    private bool sprinting;
    private float speed = 6f;
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
        horizontal = Input.GetAxisRaw("Horizontal");
        sprinting = Input.GetKey(KeyCode.LeftShift);

        animator.SetFloat("speed", Mathf.Abs(horizontal));


        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (rb.velocity.y > 0f)
        {
            animator.SetBool("isjumping", true);
        }

        if (rb.velocity.y == 0f)
        {
            animator.SetBool("isjumping", false);
        }

        if (Input.GetButtonDown("Jump") == true)
        {
            animator.SetBool("isjumping", true);
        }
        if (IsGrounded() == true)
        {
            animator.SetBool("isjumping", false);
        }
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
            speed = speed + 0.1f;
            SpeedLimit();
        }
        if (sprinting && Input.GetKey(KeyCode.C) == false)
        {
            speed = speed - 0.0001f;
            if (speed <= 6f)
            {
                speed = 6f;
            }
        }
    }
    private void Crouchingis()
    {
        if (Input.GetKey(KeyCode.C) == true && IsGrounded())
        {
            // HERE WE NEED TO ADD ADJUST FOR BOX COLIDER WHILE CROUCHING
            animator.SetBool("iscrouching", true);
            speed = 4f;
            if (Input.GetKey(KeyCode.LeftShift)){
                speed = 4f;
            }
        }
        else
        {
            animator.SetBool("iscrouching", false);
        }
    }
    private void SpeedLimit() {
        if (speed >= 12)
        {
            speed = 12;
        }
    }
    private void LooseMomentum()
    {
        if (horizontal == 0)
        {
            speed = 6f;
        }
    }
    private IEnumerator DelayS()
    {
        yield return new WaitForSeconds(1);
    }
}