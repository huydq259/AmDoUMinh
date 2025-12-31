using UnityEngine;

public class TrapsSound : MonoBehaviour
{
    [SerializeField] private float soundRadius = 5f;
    [SerializeField] private LayerMask whatIsPlayer;

    public void PlayElectricTrapSound() {
        Collider2D collInfo = Physics2D.OverlapCircle(transform.position, soundRadius, whatIsPlayer);

        if (collInfo) {
            AudioManager.instance.PlaySound("Electric");
        }
    }

    public void PlayExplosionSound() {
        Collider2D collInfo = Physics2D.OverlapCircle(transform.position, soundRadius, whatIsPlayer);

        if (collInfo) {
            AudioManager.instance.PlaySound("Explosion");
        }
    }

    public void PlayHushSound() {
        Collider2D collInfo = Physics2D.OverlapCircle(transform.position, soundRadius, whatIsPlayer);

        if (collInfo) {
            //AudioManager.instance.PlaySound("Hush");
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, soundRadius);
    }
}
