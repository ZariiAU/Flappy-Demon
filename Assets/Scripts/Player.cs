using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private ScoreKeeper scoreKeeper;
    public UnityEvent playerTouchedCheckpoint;
    public UnityEvent playerJumped;
    public UnityEvent playerDied;
    public float force;
    public bool disableInputs;

    // Start is called before the first frame update
    void Start()
    {
        scoreKeeper = ScoreKeeper.Instance;

        rb = GetComponent<Rigidbody2D>();
        
        playerDied.AddListener(() =>
        {
            // Launch the Player on death
            rb.AddForce(new Vector2(700,700));
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddTorque(50);

            // Update the Highscore
            if (scoreKeeper.Score > scoreKeeper.HighScore)
            {
                scoreKeeper.HighScore = scoreKeeper.Score;
            }
        });

        // Update the score
        playerTouchedCheckpoint.AddListener(() => { scoreKeeper.Score++; });
    }

    void Update()
    {
        // Bounce the player upwards on Spacebar
        if (Input.GetKeyDown(KeyCode.Space) && !disableInputs)
        {
            playerJumped.Invoke();
            rb.velocity = Vector2.zero;
            rb.AddForce(transform.up * force);
        }
    }

    // Check for gap between Pipes to add score
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            playerTouchedCheckpoint.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponentInParent<Pipe>())
        {
            playerDied.Invoke();
        }
    }
}
