using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private ScoreKeeper scoreKeeper;
    public float force;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(transform.up * force);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            scoreKeeper.Score++;
            Debug.Log("Score:" + scoreKeeper.Score);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponentInParent<Pipe>())
        {
            if(scoreKeeper.Score > scoreKeeper.HighScore)
            {
                scoreKeeper.HighScore = scoreKeeper.Score;
            }
            SceneManager.LoadScene(0);
        }
    }
}
