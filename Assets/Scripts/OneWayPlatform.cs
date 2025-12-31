using UnityEngine;

public class OneWayPlatform : MonoBehaviour {

    private float offsetY = 1.3f;
    private Transform player;
    private BoxCollider2D boxCollider2D;

    private void Start() {

        boxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {

        if (player == null) return;

        if (transform.position.y < player.position.y - offsetY) {
            boxCollider2D.enabled = true;
        }
        else {
            boxCollider2D.enabled = false;
        }
    }
}
