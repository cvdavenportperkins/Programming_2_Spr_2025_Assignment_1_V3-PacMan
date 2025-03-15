using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ghosts : MonoBehaviour
{
    private GameManager gameManager;
    private SoundManager soundManager;
    public Movement movement;
    public float speed = 6f;
    public Transform target; // Player target
    public Ghosts ghost;

    private void Awake()
    {
        this.ghost = GetComponent<Ghosts>();
        this.movement = GetComponent<Movement>();
    }

    void Start()
    {
        gameManager = GameManager.instance;
        soundManager = SoundManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (gameManager.isPlayerTurboActive == true)
            {
                Destroy(gameObject);
                gameManager.AddScore(200);
                soundManager.PlaySound(SoundManager.Instance.PlayerKillSFX);
                Debug.Log("Player/Ghost collision");
            }
            else
            {
                gameManager.SubtractScore(50);
                gameManager.DecreaseHealth();
                soundManager.PlaySound(SoundManager.Instance.PlayerHitSFX);
            }
        }
        else if (collision.tag == "Node")
        {
            Node node = collision.GetComponent<Node>();
            if (node != null)
            {
                Debug.Log("Ghost '" + ghost.name + "' collided with node at position " + node.transform.position + " with available directions: " + string.Join(", ", node.availableDirections));

                if (node.availableDirections.Count > 0)
                {
                    int index = Random.Range(0, node.availableDirections.Count);
                    if (node.availableDirections[index] == -this.ghost.movement.direction && node.availableDirections.Count > 1)
                    {
                        index++;

                        if (index >= node.availableDirections.Count)
                        {
                            index = 0;
                        }
                    }

                    this.ghost.movement.SetDirection(node.availableDirections[index]);
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



