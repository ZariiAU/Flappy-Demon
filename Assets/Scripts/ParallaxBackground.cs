using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    public float xMinBounds;
    public float xMaxBounds;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameStarted)
        {
            if (transform.position.x >= xMinBounds)
                transform.position = new Vector2(transform.position.x + xSpeed * Time.deltaTime, transform.position.y);
            else
            {
                // Completely unfair randomiser. TODO: Base the distance of the next pipe on the previous one
                transform.position = new Vector2(xMaxBounds, transform.position.y);
            }
        }
    }
}
