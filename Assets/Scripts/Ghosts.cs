using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ghosts : MonoBehaviour
{

    private GameManager gameManager;
    private SoundManager soundManager;
    public Movement movement;
    public float speed = 6f;
    public Transform target; //player target
    public Ghosts ghost;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        this.ghost = GetComponent<Ghosts>();
        this.movement = GetComponent<Movement>();
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

                if (node != null && node.availableDirections.Count > 0)
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

                    if (gameManager.isPlayerTurboActive)
                    {
                        //playerTurbo behavior
                        Vector2 directionToAvoidPlayer = GetDirectionToAvoidPlayer(node.availableDirections);
                        this.ghost.movement.SetDirection(node.availableDirections[index]);
                    }
                    else
                    {
                        //base behavior
                        this.ghost.movement.SetDirection(node.availableDirections[index]);
                    }
                }
                else
                {
                    Debug.LogWarning("Node has no available directions");
                }
            }
            else
            {
                Debug.LogError("Node component is null");
            }
            
        }
    }


    private Vector2 GetDirectionToAvoidPlayer(List<Vector2> availableDirections)
    {
        Vector2 playerPosition = target.position;
        Vector2 ghostPosition = this.transform.position;

        Vector2 directionToPlayer = playerPosition - ghostPosition;
        Vector2 oppositeDirection = -directionToPlayer;

        Vector2 bestDirection = availableDirections[0];
        float maxDotProduct = float.NegativeInfinity;

        foreach (Vector2 direction in availableDirections)
        {
            float dotProduct = Vector2.Dot(direction, oppositeDirection);
            if (dotProduct > maxDotProduct)
            {
                maxDotProduct = dotProduct;
                bestDirection = direction;
            }
        }
        return bestDirection;


    }
}

       
