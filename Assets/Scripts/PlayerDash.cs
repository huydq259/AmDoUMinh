using UnityEngine;

public class PlayerDash : MonoBehaviour {

    public float dashForce = 15f;
    public float dashDuration = 0.2f;

    private Rigidbody2D rb;
    private Animator animator;
    private Player player;
    private bool isDashing;
    private float originalGravity;
    private bool facingRight = true;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        animator = this.GetComponent<Animator>();
        player = this.gameObject.GetComponent<Player>();
    }

    void Update() {
        // Flip player based on horizontal input
        float move = Input.GetAxisRaw("Horizontal");
        if (move > 0) facingRight = true;
        else if (move < 0) facingRight = false;

        // Trigger dash
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Backspace) && !isDashing) {
            StartCoroutine(Dash());
            animator.SetBool("Dash", true);
            AudioManager.instance.PlaySound("Dash");
        }
    }

    System.Collections.IEnumerator Dash() {
        //animator.SetTrigger("Dash");

        if (!player.isGround) {
            isDashing = true;
            // Disable gravity
            rb.gravityScale = 0f;

            // Calculate dash direction
            float dashDir = facingRight ? 1f : -1f;

            // Set dash velocity
            rb.linearVelocity = new Vector2(dashDir * dashForce, 0f);
        }

        // Wait dash duration
        yield return new WaitForSeconds(dashDuration);

        // Stop dash
        rb.linearVelocity = Vector2.zero;

        // Restore gravity
        rb.gravityScale = originalGravity;

        isDashing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) {
            animator.SetBool("Dash", false);
        }
    }
}



