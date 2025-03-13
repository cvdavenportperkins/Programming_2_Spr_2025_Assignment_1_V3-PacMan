using UnityEngine;

public class Passage : MonoBehaviour
{
    public Transform exit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 position = other.transform.position;
        position.x = this.exit.position.x;
        position.y = this.exit.position.y;

        other.transform.position = position;
    }
}
