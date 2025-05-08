using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{

    public static GhostBehavior instance;
    private GameManager gameManager;
    private SoundManager soundManager;
    public Ghosts ghosts;
    public enum GhostState { Scatter, Chase, Frightened, Eaten }
    public GhostState currentState;

    public GhostChase ghostChase;
    public GhostFrightened ghostFrightened;
    public GhostHome ghostHome;
    public GhostScatter ghostScatter;

    public Movement movement;
    public Transform homePosition;
    public Transform target;
    public float baseSpeed = 6f;
    public float frightenedSpeed = 3f;

    private void Awake()
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

        // Make GhostBehavior persistent across scenes
        DontDestroyOnLoad(gameObject);

        movement = GetComponent<Movement>();
        gameManager = GameManager.instance;
        soundManager = SoundManager.Instance;

    }
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetState(GhostState.Scatter);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GhostState.Scatter:
                MoveToCorner();
                break;

            case GhostState.Chase:
                ChaseTarget();
                break;

            case GhostState.Frightened:
                EvadeTarget();
                break;

            case GhostState.Eaten:
                ReturnHome();
                break;
        }
    }

    public void SetState(GhostState newState)
    {
        currentState = newState;
        movement.speed = (newState == GhostState.Frightened) ? frightenedSpeed : baseSpeed;

    }

    private void MoveToCorner()
    {
        movement.SetDirection(target.position - transform.position);
    }

    private void ChaseTarget()
    {
        movement.SetDirection(gameManager.playerPrefab.transform.position - transform.position);
    }

    private void EvadeTarget()
    {
        movement.SetDirection(transform.position - gameManager.playerPrefab.transform.position);
    }

    private void ReturnHome()
    {
        movement.SetDirection(homePosition.position - transform.position);
        if (Vector2.Distance(transform.position, homePosition.position) < 0.5f)
        {
            SetState(GhostState.Scatter);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentState = (collision.tag == "PowerPellet") ? GhostState.Frightened :
                       (collision.tag == "Player" && currentState == GhostState.Frightened) ? GhostState.Eaten :
                       currentState;
        if (collision.tag == "Player" && currentState == GhostState.Eaten)
        {
            gameManager.AddScore(250);
            soundManager.PlaySound(SoundManager.Instance.PlayerKillSFX);
            SetState(GhostState.Eaten);
        }

    }
}
