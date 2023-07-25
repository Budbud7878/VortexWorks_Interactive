using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    public SpriteRenderer sc;
    public Sprite newsprite;
    public Sprite newsprite2;

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

        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        Flip();
    }
    
    private void FixedUpdate()
    {
        // Movement speed for player
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        if (horizontal != 0)
        {
            ChangeSprite2();
        }
        else { 
            ChangeSprite(); 
        }
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

    //Changes the sprite
    void ChangeSprite()
    {
        sc.sprite = newsprite;
    }
    void ChangeSprite2()
    {
        sc.sprite = newsprite2;
    }
}
