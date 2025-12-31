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

            RaycastHit2D hitInfo = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, distance, whatIsGround);

            if (hitInfo == false) {

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
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.tag == "Arrow") {
            TakeDamage(1);
            Destroy(other.gameObject);
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
