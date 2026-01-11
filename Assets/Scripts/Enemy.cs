using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3;

    public float walkSpeed = 1.5f;
    public Transform groundCheckPoint;
    public float distance = .3f;
    public LayerMask whatIsGround;
    private bool facingLeft;

    public float attackRangeRadius = 6f;
    public LayerMask whatIsPlayer;
    public Transform player;
    public float chaseSpeed = 2f;
    public float retrieveDistance = 3f;
    private Animator animator;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    public GameObject floatingTextPrefab;
    public Transform textSpawnPoint;

    public Transform attackPoint;
    public float attackRadius = 1f;
    public Vector3 offset;
    public Transform wallCheckPoint; // Điểm kiểm tra tường
    public float wallCheckDistance = 0.5f; // Khoảng cách kiểm tra

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        facingLeft = true;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        animator = this.gameObject.GetComponent<Animator>();
        boxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) {
            animator.SetBool("Attack", false);
        }

        if (player != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (maxHealth <= 0) {
            animator.SetBool("Attack", false);
            Die();
            return;
        }

        Collider2D collInfo = Physics2D.OverlapCircle(transform.position + offset, attackRangeRadius, whatIsPlayer);

        if (collInfo) {

            if (player.position.x > transform.position.x && facingLeft) {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft = false;
            }
            else if (player.position.x < transform.position.x && facingLeft == false){
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                facingLeft = true;
            }

            Vector2 targetPos = new Vector2(player.position.x, transform.position.y);

            if (Vector2.Distance(transform.position, targetPos) > retrieveDistance) {
                animator.SetBool("Attack", false);
                transform.position = Vector2.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
            }
            else {
                animator.SetBool("Attack", true);
            }  
        }
        else {
            transform.Translate(Vector2.left * Time.deltaTime * walkSpeed);
            // 1. Kiểm tra mặt đất (logic cũ)
            RaycastHit2D groundInfo = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, distance, whatIsGround);

            // 2. Kiểm tra tường (logic MỚI)
            // Bắn tia sang trái hoặc phải tùy theo hướng đang nhìn
            Vector2 wallCheckDir = facingLeft ? Vector2.left : Vector2.right;
            RaycastHit2D wallInfo = Physics2D.Raycast(wallCheckPoint.position, wallCheckDir, wallCheckDistance, whatIsGround);
            // Điều kiện quay đầu: (Hết đường HOẶC Đụng tường)
            if (groundInfo == false || wallInfo == true)
            {

                if (facingLeft)
                {
                    transform.eulerAngles = new Vector3(0f, -180f, 0f);
                    facingLeft = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    facingLeft = true;
                }
            }
        }
    }

    public void Attack() {

        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsPlayer);

        if (collInfo) {
            
            if (collInfo.gameObject.GetComponent<Player>() != null) {
                Player.instance.TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int damageAmount) {

        if (maxHealth <= 0) {
            //animator.SetTrigger("Death");
            return;
        }
        maxHealth -= damageAmount;
        animator.SetTrigger("Hurt");
        CameraShake.instance.Shake(2.5f, .15f);
        Instantiate(floatingTextPrefab, textSpawnPoint.position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected() {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + offset, attackRangeRadius);

        if (groundCheckPoint == null) {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(groundCheckPoint.position, Vector2.down * distance);

        if (attackPoint == null) {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        if (wallCheckPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(wallCheckPoint.position, (facingLeft ? Vector2.left : Vector2.right) * wallCheckDistance);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.tag == "Arrow") {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Nếu đụng vào vật thể có Rigidbody2D thì quay đầu
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null) {
            if (facingLeft) {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft = false;
            }
            else {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                facingLeft = true;
            }
        }
    }

    public void ShakeCamera() {
        CameraShake.instance.Shake(4f, .18f);
    }

    void Die() {
        Debug.Log(this.gameObject.name + " Died!");
        //isEnemyDied = true;
        animator.SetBool("Death", true);
        rb.gravityScale = 0f;
        boxCollider2D.enabled = false;
        Destroy(this.gameObject, 5f);
    }
}
