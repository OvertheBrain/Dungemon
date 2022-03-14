using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    private Rigidbody2D rb;
    public int direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        switch (direction)
        {
            case 0:
                rb.velocity = new Vector2(speed, 0);
                break;
            case 1:
                rb.velocity = new Vector2(-speed, 0);
                break;
            case 2:
                rb.velocity = new Vector2(0, speed);
                break;
            case 3:
                rb.velocity = new Vector2(0, -speed);
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
