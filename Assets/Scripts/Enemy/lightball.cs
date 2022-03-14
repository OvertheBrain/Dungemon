using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightball : MonoBehaviour
{
    private Rigidbody2D rb;
    public int direction;
    public float speed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //direction = Player.GetComponent<PlayerController>().direction;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        rb.velocity = new Vector2(speed * direction, rb.velocity.y);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
