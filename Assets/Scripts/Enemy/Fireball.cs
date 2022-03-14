using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Animator anim;

    public int dir;
    Vector2[] directions =
    {
        new Vector2(2f,1).normalized,
        new Vector2(1, 0),
        new Vector2(2.5f, -1).normalized,
        new Vector2(-2f,1).normalized,
        new Vector2(-1, 0),
        new Vector2(-2.5f, -1).normalized,
    };

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        speed = 15f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (dir > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            rb.velocity = speed * directions[dir - 1];
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            rb.velocity = speed * directions[-dir + 2];
        }
        if (anim.GetBool("isExplode"))
            rb.velocity = Vector2.zero;
    }

    IEnumerator Explode()
    {        
        anim.SetBool("isExplode", true);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);                      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {            
            StartCoroutine(Explode());
            
        }
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Explode());
        }
    }
}

