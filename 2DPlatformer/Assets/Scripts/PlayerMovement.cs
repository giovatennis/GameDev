using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    float moveSpeed = 5f;
    Rigidbody2D rb;

    float jumpForce = 5f;

    int maxJumps = 2;
    int jumpsRemaining;


    public int collected = 0;

    public void Collect()
    {
        collected++;
        Debug.Log("Collected: " + collected);
    }

    SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jumpsRemaining = maxJumps;
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = 0f;

        if(Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            moveInput = -1f;
        }
        else if(Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            moveInput = 1f;
        }
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if(Keyboard.current.spaceKey.wasPressedThisFrame && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,jumpForce);
            jumpsRemaining--;
        }
        if(moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if(moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            jumpsRemaining = maxJumps;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Water"))
        {
            RestartLevel();
        }
        if(other.CompareTag("Goal") && collected == 4)
        {
            Debug.Log("You Win!");
        }
        if(other.CompareTag("Enemy"))
        {
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
