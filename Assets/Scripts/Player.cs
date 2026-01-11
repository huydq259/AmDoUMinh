using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player instance;
    public int maxHealth = 5;
    private Animator animator;
    public Rigidbody2D rb;
    public float jumpHeight = 7f;
    public float moveSpeed = 5f;

    private float movement;
    [HideInInspector] public bool isGround;
    private bool facingRight;

    public Transform groundCheckPoint;
    public float groundCheckRadius = .2f;
    public LayerMask whatIsGround;

    public GameObject arrowPrefab;
    public Transform spawnPosition;
    public float arrowSpeed = 7f;

    public GameObject explosionPrefab;
    public Transform explosionSpawnPoint;

    private int currentDiamonds;
    public GameObject collect_EffectPrefab;

    public Text currentHeart_Text;
    public Text currentDiamond_Text;

    private int jumpCount;
    public int totallJumps = 2;
    private bool isVictory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGround = true;
        facingRight = true;
        animator = this.gameObject.GetComponent<Animator>();

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(this);
        }

        currentDiamonds = 0;
        jumpCount = totallJumps;
        isVictory = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (maxHealth <= 0) {
            Die();
        }

        if (isVictory) {
            animator.SetFloat("Run", 0f);
            return;
        }

        currentHeart_Text.text = maxHealth.ToString();
        movement = Input.GetAxis("Horizontal");


        Collider2D collInfo = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);
        if (collInfo) {
            isGround = true;
        }


        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

        Flip();
        PlayRunAnimation();

        if (Input.GetMouseButtonDown(0)) {
            animator.SetTrigger("Fire");
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement * moveSpeed, rb.linearVelocity.y);
    }

    public void FireArrow() {
        GameObject tempArrowPrefab = Instantiate(arrowPrefab, spawnPosition.position, spawnPosition.rotation);
        tempArrowPrefab.GetComponent<Rigidbody2D>().linearVelocity = spawnPosition.right * arrowSpeed;
    }

    void PlayRunAnimation() { 
        
        if (Mathf.Abs(movement) > 0f) {
            animator.SetFloat("Run", 1f);
        }
        else if (movement < 0.1f) {
            animator.SetFloat("Run", 0f);
        }
    }

    void Flip() { 
        
        if (movement < 0f && facingRight == true) {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && facingRight == false) {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }
    }

    void Jump()
    {
        // Chỉ cần còn lượt nhảy là được nhảy (bất kể đang dưới đất hay trên trời)
        if (jumpCount > 0)
        {
            // 1. Tạo lực nhảy
            Vector2 velocity = rb.linearVelocity;
            velocity.y = jumpHeight;
            rb.linearVelocity = velocity;

            // 2. Cập nhật trạng thái
            isGround = false; // Rời mặt đất
            animator.SetBool("Jump", true);
            AudioManager.instance.PlaySound("Dash");

            // 3. Trừ lượt nhảy đi 1
            jumpCount--;
        }
    }

    public void TakeDamage(int damageAmount) { 
        
        if (maxHealth <= 0) {
            return;
        }
        else {
            maxHealth -= damageAmount;
            animator.SetTrigger("Hurt");
            CameraShake.instance.Shake(2f, .12f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") {
            animator.SetBool("Jump", false);
            jumpCount = totallJumps;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll) {

        if (coll.gameObject.CompareTag("DeadLine")) {
            Die();
        }

        if (coll.gameObject.CompareTag("Trap")) {
            TakeDamage(1);
        }

        if (coll.gameObject.CompareTag("Chest")) {
            isVictory = true;
            GameManager.instance.TriggerVictoryUI();
        }

        if (coll.gameObject.tag == "Heart") {
            maxHealth++;
            AudioManager.instance.PlaySound("Collect");
            currentHeart_Text.text = maxHealth.ToString();
            GameObject tempCollect_Effect = Instantiate(collect_EffectPrefab, coll.gameObject.transform.position, Quaternion.identity);
            Destroy(tempCollect_Effect, .401f);
            Destroy(coll.gameObject);
        }
        
        if (coll.gameObject.tag == "Arrow_Enemy") {
            TakeDamage(1);
            Destroy(coll.gameObject);
        }

        if (coll.gameObject.tag == "Diamond") {
            currentDiamonds++;
            AudioManager.instance.PlaySound("Collect");
            currentDiamond_Text.text = currentDiamonds.ToString();
            GameObject tempCollect_Effect = Instantiate(collect_EffectPrefab, coll.gameObject.transform.position, Quaternion.identity);
            Destroy(tempCollect_Effect, .401f);
            Destroy(coll.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null) {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }

    void Die() {
        Debug.Log(this.gameObject.name + " Died!");
        //CameraShake.instance.Shake(4f, .18f);
        AudioManager.instance.PlaySound("Explosion");
        GameObject tempExplosion = Instantiate(explosionPrefab, explosionSpawnPoint.position, Quaternion.identity);
        Destroy(tempExplosion, .901f);
        GameManager.instance.TriggerGameOverUI();
        Destroy(this.gameObject);
    }
}
