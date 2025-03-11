using UnityEngine;

public class PacManController : MonoBehaviour
{
    public static PacManController instance;
    public float speed = 5f;
    private Vector2 direction;
    private GameManager gameManager;
    private Movement movement;


    void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            instance = this; // Set instance to this
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
        }

        // Make GameManager persistent across scenes
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        gameManager = GameManager.instance; // Access singleton instance
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        // Get player input
        if (Input.GetKey(KeyCode.W))
        {
            movement.SetDirection(Vector2.up);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            movement.SetDirection(Vector2.down);
        }
        
        if (Input.GetKey(KeyCode.A)) 
        {
            movement.SetDirection(Vector2.left);
        }
        
        if (Input.GetKey(KeyCode.D)) 
        {
            movement.SetDirection(Vector2.right);
        }
        
        if (Input.GetKey(KeyCode.UpArrow)) 
        {
            movement.SetDirection(Vector2.up);
        }
        
        if (Input.GetKey(KeyCode.DownArrow)) 
        {
            movement.SetDirection(Vector2.down);
        }
        
        if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            movement.SetDirection(Vector2.left);
        }
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movement.SetDirection(Vector2.right);
        }
        Debug.Log("Input direction: " + movement.direction);
    }

    void FixedUpdate()
    {
        // Move Pac-Man
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pellet")
        {
            gameManager.CollectPellet(collision.gameObject);
            Destroy(collision.gameObject);
            gameManager.AddScore(10); // Add points for each pellet
            SoundManager.Instance.PlaySound(SoundManager.Instance.ScoreUpSFX); // Play sound effect
        }
        
        else if (collision.tag == "PowerPellet")
        {
            Destroy(collision.gameObject);
            gameManager.AddScore(50); // Add more points for power pellet
            SoundManager.Instance.PlayPowerupSound(SoundManager.Instance.PlayerTurboSFX,10f); // Play sound effect
            SoundManager.Instance.PlaySound(SoundManager.Instance.EnemyMoveSFX);
            gameManager.ActivatePlayerTurbo(10f); //Activate PlayerTurbo for 10 seconds
        }

       
    }
}

