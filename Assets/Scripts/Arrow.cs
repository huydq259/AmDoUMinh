using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float arrowSpeed = 5f;

    private void Start()
    {
        Destroy(this.gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu mũi tên chạm vào Ground thì hủy
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
    }
}
