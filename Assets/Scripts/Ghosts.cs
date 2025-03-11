using UnityEngine;

public class Ghosts : MonoBehaviour
{

    private GameManager gameManager;
    private SoundManager soundManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
}
