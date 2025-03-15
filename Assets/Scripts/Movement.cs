using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public LayerMask obstacleLayer;
    public Vector2 initialDirection;
    public Vector2 direction;
    public Vector2 nextDirection;

    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.direction = initialDirection;
    }

    private void Update()
    {
        if (this.nextDirection != Vector2.zero)
        {
            SetDirection(this.nextDirection);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = this.rb.position;
        Vector2 translation = this.direction * PacManController.instance.speed * Time.fixedDeltaTime;
        this.rb.MovePosition(position + translation);

        Debug.Log("Current Position: " + position);
        Debug.Log("Translation: " + translation);
    }

    public void SetDirection(Vector2 direction)
    {
        if (!Blocked(direction))
        {
            this.direction = direction;
            this.nextDirection = Vector2.zero;
        }
        else
        {
            this.nextDirection = direction;
        }
    }

    public bool Blocked(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.5f, 0.0f, direction, 1.5f, this.obstacleLayer);
        Debug.Log("Blocked: " + (hit.collider != null) + " in direction: " + direction);
        return hit.collider != null;
    }
}
