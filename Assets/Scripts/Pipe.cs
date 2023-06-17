using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    public float xMinBounds;
    public float xMaxBounds;
    public float yMinBounds;
    public float yMaxBounds;
    public bool isFirstPipe;

    private void Start()
    {
        if(!isFirstPipe)
            transform.position = new Vector2(transform.position.x, Random.Range(yMinBounds + 1, yMaxBounds - 1));
    }

    void Update()
    {
        if (GameManager.Instance.gameStarted)
        {
            // Move the pipes
            if (transform.position.x >= xMinBounds)
                transform.position = new Vector2(transform.position.x + xSpeed * Time.deltaTime, transform.position.y);
            else
            {
                // Completely unfair randomiser. TODO: Base the distance of the next pipe on the previous one
                transform.position = new Vector2(xMaxBounds + 1, Random.Range(yMinBounds + 1, yMaxBounds - 1));
            }
        }
    }
}
