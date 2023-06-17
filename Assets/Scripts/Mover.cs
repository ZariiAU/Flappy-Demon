using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public Vector2 direction;
    public float speed = 100;
    public float rotationSpeed = 100;
    public bool shouldMove = false;

    void Update()
    {
        if(shouldMove)
            Move();
    }

    [ContextMenu("Move")]
    public void Move()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y) + direction * speed * Time.deltaTime;
        transform.Rotate(new Vector3 (transform.rotation.x, 0, transform.rotation.z + direction.y) * rotationSpeed * Time.deltaTime);
    }
}
