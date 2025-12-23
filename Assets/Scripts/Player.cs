using UnityEngine;
using UnityEngine.SceneManagement;

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
<<<<<<< HEAD
    private int currentHearts;
    private int currentGolds;
=======

    public GameObject arrowPrefab;
    public Transform spawnPosition;
    public float arrowSpeed = 7f;
>>>>>>> 41deef5ae23f9cc495764686c47223140f4fed6b
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGround = true;
        facingRight = true;
        animator = this.gameObject.GetComponent<Animator>();
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
        if (collInfo == true)
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
        GameObject tempArrowPrefab = Instantiate(arrowPrefab, spawnPosition.position, spawnPosition.rotation);
        tempArrowPrefab.GetComponent<Rigidbody2D>().linearVelocity = spawnPosition.right * arrowSpeed;
    }
    void PlayRunAnimation()
    {
        if (Mathf.Abs(movement) > 0f)
        {
            animator.SetFloat("Run", 1f);
        }
        else if (movement < 0.1f)
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
            Vector2 velocity = rb.linearVelocity;
            velocity.y = jumpHeight;
            rb.linearVelocity = velocity;
            isGround = false;
            animator.SetBool("Jump", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            animator.SetBool("Jump", false);
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            Die();
        }
    }
    void Die()
    {
        // In ra log để kiểm tra
        Debug.Log("Nhân vật đã rơi xuống vực!");

        // Load lại Scene hiện tại ngay lập tức
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
        if (collision.gameObject.tag == "Heart")
        {
            currentHearts ++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Gold")
        {
            currentGolds ++;
            Destroy(collision.gameObject);
        }
    }
}
