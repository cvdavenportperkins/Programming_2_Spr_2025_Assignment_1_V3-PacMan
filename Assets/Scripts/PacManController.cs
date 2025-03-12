using UnityEngine;

public class PacManController : MonoBehaviour
{
    public static PacManController instance;
    public GameManager gameManager;
    private Vector2 direction;
    public float speed = 5f;
    public Movement movement;


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

        this.movement = GetComponent<Movement>();
     
        if (gameManager == null)
        {
            gameManager = Object.FindFirstObjectByType<GameManager>();
        }
    }

    void Update()
    {
        UpdateDirection();
        Debug.Log("Input direction: " + movement.direction);
    }
    
    void FixedUpdate()
    {
        UpdateDirection();
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
    }
        
    void UpdateDirection()
    {   
        // Get player input
        if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.UpArrow)))
        {
            direction = Vector2.up;
            this.movement.SetDirection(Vector2.up);
        }
        
        else if ((Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.DownArrow)))
        {
            direction = Vector2.down;
            this.movement.SetDirection(Vector2.down);
        }
        
        else if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftArrow))) 
        {
            direction = Vector2.left;
            this.movement.SetDirection(Vector2.left);
        }
        
        else if ((Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.RightArrow)))
        {
            direction = Vector2.right;
            this.movement.SetDirection(Vector2.right);
        }
          
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("GameManager is " + (gameManager != null ? "assigned" : "null"));
        
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

