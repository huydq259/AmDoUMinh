using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float arrowSpeed = 5f;

    private void Start()
    {
        Destroy(this.gameObject, 5f);
    }
}
