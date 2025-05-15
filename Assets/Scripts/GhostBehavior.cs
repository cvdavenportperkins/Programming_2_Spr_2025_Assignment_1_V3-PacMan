using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    
    private GameManager gameManager;
    private SoundManager soundManager;
    public Ghosts ghosts;
    public enum GhostState { Scatter, Chase, Frightened, Eaten }
    public GhostState currentState;

    public Movement movement;
    public Transform homePosition;
    public Transform target;
    public float baseSpeed = 6f;
    public float frightenedSpeed = 3f;
    public float eatenSpeed = 9f;

    private void Awake()
    {

        movement = GetComponent<Movement>();
        if (movement == null)
        {
            Debug.LogError("GhostBehavior: Movement component is missing");
        }
        
        gameManager = GameManager.instance;
        soundManager = SoundManager.Instance;

    }
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager instance is missing");
        }
        else
        {
            gameManager = GameManager.instance;
            target = GameManager.instance.playerPrefab.transform;
        }


        if (SoundManager.Instance == null)
        {
            Debug.LogError("SoundManager instance is missing");
        }
        else
        {
            soundManager = SoundManager.Instance;
        }

        if (GameManager.instance != null && GameManager.instance.playerPrefab != null)
        {
            target = GameManager.instance.playerPrefab.transform;
        }
        else
        {
            Debug.LogError("GhostBehavior: target is null");
        }

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
        if (movement != null)
        {
            movement.speed = (newState == GhostState.Frightened) ? frightenedSpeed :
                         (newState == GhostState.Eaten) ? eatenSpeed :
                         baseSpeed;

        }
        else
        {
            Debug.LogError("GhostBehavior: movement is null");
        }
    
    }

    private void MoveToCorner()
    {
        if (target != null)
        {
            movement.SetDirection((target.position - transform.position).normalized);
        }
        else
        {
            Debug.LogError("GhostBehavior: target is null");
        }
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
        movement.speed = eatenSpeed; 
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
