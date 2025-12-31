using UnityEngine;

public class Enemy2 : MonoBehaviour {

    public int maxHealth = 3;
    private bool facingLeft;

    public float attackRangeRadius = 6f;
    public LayerMask whatIsPlayer;
    public Transform player;
    private Animator animator;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    public GameObject floatingTextPrefab;
    public Transform textSpawnPoint;

    [Header("Attack")]
    public Transform firePoint;
    public GameObject arrowPrefab;
    public float arrowVelocity = 10f;
    private bool isPlayerInAttackRange;
    public Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        facingLeft = true;
        isPlayerInAttackRange = false;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        animator = this.gameObject.GetComponent<Animator>();
        boxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update() {

        if (player == null) {
            animator.SetBool("Attack", false);
        }

        if (player != null) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    
        if (maxHealth <= 0) {
            animator.SetBool("Death", true);
            Die();
            return;
        }

        Collider2D collInfo = Physics2D.OverlapCircle(transform.position + offset, attackRangeRadius, whatIsPlayer);

        if (collInfo) {
            animator.SetBool("Attack", true);
            isPlayerInAttackRange = true;
        }
        else {
            isPlayerInAttackRange = false;
            animator.SetBool("Attack", false);
        }

        if (isPlayerInAttackRange) {

            if (transform.position.x < player.position.x && facingLeft) {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft = false;
            }
            else if (transform.position.x > player.position.x && facingLeft == false){
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                facingLeft = true;
            }
        }
    }

    public void FireArrow() {
        GameObject tempArrowPrefab = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        tempArrowPrefab.gameObject.GetComponent<Rigidbody2D>().linearVelocity = -firePoint.right * arrowVelocity;
        Destroy(tempArrowPrefab, 5f);
    }

    public void TakeDamage(int damageAmount) {

        if (maxHealth <= 0) {
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
