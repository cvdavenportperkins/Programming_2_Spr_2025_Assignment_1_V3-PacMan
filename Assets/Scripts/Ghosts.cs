using UnityEngine;

public class Ghosts : MonoBehaviour
{

    private GameManager gameManager;
    private SoundManager soundManager;
    public Movement movement;
    public float speed = 6f;
    public Transform target;
    public Ghosts ghost;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private void Awake()
    {
        this.ghost.GetComponent<Ghosts>();
        this.movement.GetComponent<Movement>();
    }
    
    
    void Start()
    {
        gameManager = GameManager.instance; //Access singleton instance
        soundManager = SoundManager.Instance;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") //check game object tag for "player"
        {
            if (gameManager.isPlayerTurboActive)
            {
                Destroy(gameObject);
                speed = 7f;
                gameManager.AddScore(200);
                soundManager.PlaySound(SoundManager.Instance.PlayerKillSFX);
            }
            else
            {
                gameManager.SubtractScore(50);
                soundManager.PlaySound(SoundManager.Instance.PlayerHitSFX);
            }
        }
    }
//Base Behavior



//PlayerTurbo Behavior



//Chase Behavior



//Home




//Reset





}
