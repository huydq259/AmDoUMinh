using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpHeight = 7f;
    public float moveSpeed = 5f;
    private float movement;

    private bool facingRight;
    private Animator animator;
    public Transform groundCheckPoint;
    public float groundCheckRadius = .2f;
    public LayerMask whatIsGround;
    private bool isGround;

    private int currentHearts;
    private int currentGolds;

    public GameObject arrowPrefab;
    public Transform spawnPosition;
    public float arrowSpeed = 7f;

    public Text currentHeart_Text;
    public Text currentGold_Text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGround = true;
        facingRight = true;
        animator = GetComponent<Animator>();
        currentHearts = 0;
        currentGolds = 0;
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        Collider2D collInfo = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);
        if (collInfo != null)
        {
            isGround = true;
        }
        Flip();
        PlayRunAnimation();

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Shoot");
        }
    }
    private void FixedUpdate()
    {
        transform.position += new Vector3(movement * moveSpeed, 0f, 0f) * Time.fixedDeltaTime;
    }

    public void FireArrow()
    {
        if (arrowPrefab == null || spawnPosition == null) return;
        GameObject temp = Instantiate(arrowPrefab, spawnPosition.position, spawnPosition.rotation);
        Rigidbody2D rbArrow = temp.GetComponent<Rigidbody2D>();
        if (rbArrow != null)
        {
            Vector2 dir = (Vector2)spawnPosition.right;
            rbArrow.linearVelocity = dir * arrowSpeed;
        }
    }
    void PlayRunAnimation()
    {
        if (Mathf.Abs(movement) > 0f)
        {
            animator.SetFloat("Run", 1f);
        }
        else
        {
            animator.SetFloat("Run", 0f);
        }
    }
    void Flip()
    {
        if (movement < 0f && facingRight == true)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && facingRight == false)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }
    }
    void Jump()
    {
        if (isGround == true)
        {
            Vector2 velocity = rb != null ? rb.linearVelocity : Vector2.zero;
            velocity.y = jumpHeight;
            if (rb != null) rb.linearVelocity = velocity;
            isGround = false;
            animator.SetBool("Jump", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("Jump", false);
            isGround = true;
        }
    }
  
    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            Die();
        }

        if (collision.gameObject.CompareTag("Heart"))
        {
            currentHearts ++;
            currentHeart_Text.text = currentHearts.ToString();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Gold"))
        {
            currentGolds ++;
            currentGold_Text.text = currentGolds.ToString();
            Destroy(collision.gameObject);
        }
    }
}
