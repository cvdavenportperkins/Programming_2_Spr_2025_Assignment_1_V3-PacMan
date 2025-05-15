using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ghosts : MonoBehaviour
{
    private GameManager gameManager;
    private SoundManager soundManager;
    public GhostBehavior ghostBehavior;

    public Movement movement;
    public Transform target; // Player target
    public Ghosts ghosts;

    private void Awake()
    {
        this.ghosts = GetComponent<Ghosts>();
        this.movement = GetComponent<Movement>();
        this.ghostBehavior = GetComponent<GhostBehavior>();
    }

    void Start()
    {
        if(GameManager.instance == null)
        {
            Debug.LogError("GameManager instance is missing");
        }
        else
        {
            gameManager = GameManager.instance;
        }


        if (SoundManager.Instance == null)
        {
            Debug.LogError("GameManager instance is missing");
        }
        else
        {
            soundManager = SoundManager.Instance;
        }

        ResetState();
    }

    public void ResetState()
    {
        this.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (ghostBehavior != null)
        {
            movement.speed = ghostBehavior.currentState == GhostBehavior.GhostState.Frightened ?
                             ghostBehavior.frightenedSpeed :
                             ghostBehavior.baseSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (gameManager.isPlayerTurboActive == true)
            {
                Destroy(gameObject);
                gameManager.AddScore(250);
                soundManager.PlaySound(SoundManager.Instance.PlayerKillSFX);
                Debug.Log("Player/Ghost collision");
            }
            else
            {
                gameManager.SubtractScore(200);
                gameManager.DecreaseHealth();
                soundManager.PlaySound(SoundManager.Instance.PlayerHitSFX);
            }
        }
        else if (collision.tag == "Node")
        {
            Node node = collision.GetComponent<Node>();
            if (node != null)
            {
                Debug.Log("Ghost '" + ghosts.name + "' collided with node at position " + node.transform.position + " with available directions: " + string.Join(", ", node.availableDirections));

                if (node.availableDirections.Count > 0)
                {
                    int index = Random.Range(0, node.availableDirections.Count);
                    if (node.availableDirections[index] == -this.ghosts.movement.direction && node.availableDirections.Count > 1)
                    {
                        index++;

                        if (index >= node.availableDirections.Count)
                        {
                            index = 0;
                        }
                    }

                    this.ghosts.movement.SetDirection(node.availableDirections[index]);
                }
                else
                {
                    Debug.LogWarning("Node has no available directions.");
                }
            }
            else
            {
                Debug.LogError("Node component is null.");
            }
        }
    }
}



