using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int life = 3;
    public float speed = 0.5f;

    private Rigidbody2D rb;
    private Animator anim;

    private bool isCollide, onLeft, onRight;
    private Vector3 StartPos, EndPos;

    Vector2[] directions = { Vector2.zero, Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    private double leftpoint, rightpoint, uppoint, downpoint;
    private bool FaceLeft = true;
    private int CurrentDir = 0;

    private float PassedTime = 2;
    public float idletime = 0;

    public GameObject food, cool, exp, bomb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        leftpoint = transform.position.x - 2;
        rightpoint = transform.position.x + 2;
        uppoint = transform.position.y + 2;
        downpoint = transform.position.y - 2;
    }

    // Update is called once per frame
    void Update()
    {
        CollideMove();
        Move();
        Die();

    }

    void Move()
    {
        Vector2 dir = directions[CurrentDir];
        rb.velocity = speed * dir;

        if (CurrentDir == 0)
        {
            anim.SetBool("isMove", false);
            idletime += Time.deltaTime;
            if (idletime >= PassedTime)
            {
                idletime = 0;
                CurrentDir++;
            }
        }
        if(CurrentDir > 0)
            anim.SetBool("isMove", true);

        if(dir.x == 1)
        {
            FaceLeft = false;
        }
        else if(dir.x == -1)
        {
            FaceLeft = true;
        }

        if (FaceLeft)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        if(transform.position.x < leftpoint)
        {
            rb.velocity = new Vector2(speed, 0);
            FaceLeft = false;            
            CurrentDir++;
            if (CurrentDir > 4) CurrentDir = 0;
        }
        if (transform.position.x > rightpoint)
        {
            rb.velocity = new Vector2(-speed, 0);
            FaceLeft = true;            
            CurrentDir++;
            if (CurrentDir > 4) CurrentDir = 0;
        }
        if (transform.position.y < downpoint)
        {
            rb.velocity = new Vector2(0, speed);           
            CurrentDir++;
            if (CurrentDir > 4) CurrentDir = 0;
        }
        if (transform.position.y > uppoint)
        {
            rb.velocity = new Vector2(0, -speed);
            CurrentDir++;
            if (CurrentDir > 4) CurrentDir = 0;
        }
    }


    public void KickboardCollide(GameObject player)
    {
        Hurt();
        life--;
        isCollide = true;
        if(transform.position.x < player.transform.position.x)
        {
            onRight = true;
            EndPos = new Vector3(transform.position.x-2, transform.position.y, transform.position.z);
        }
        else
        {
            onLeft = true;
            EndPos = new Vector3(transform.position.x+2, transform.position.y, transform.position.z);
        }
        StartPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private void CollideMove()
    {
        if (isCollide)
        {
            if (onRight)
            {
                transform.position = Vector3.MoveTowards(transform.position,
        new Vector3(EndPos.x, EndPos.y, EndPos.z),
        7 * Time.deltaTime);
            }
            if (onLeft)
            {
                transform.position = Vector3.MoveTowards(transform.position,
        new Vector3(EndPos.x, EndPos.y, EndPos.z),
        7 * Time.deltaTime);                
            }

            if (transform.position.x == EndPos.x)
                 isCollide = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LightBall"))
        {
            Destroy(other.gameObject);
            Hurt();
            life--;
        }
        if (other.CompareTag("Explosion"))
        {
            life -= 3;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public void Hurt()
    {
        anim.SetTrigger("Hurt");
        life--;
    }

    private void Die()
    {
        if (life <= 0)
        {
            GameManager.instance.score += 100;
            int prop = Random.Range(0, 20);
            if (prop <= 9)
                GenerateItem();
            GameObject Dungemon = GameObject.FindGameObjectWithTag("Friend");
            GameObject Player = GameObject.FindGameObjectWithTag("Player");
            if ((Player.transform.position - Dungemon.transform.position).magnitude <= 5)
            {
                Dungemon.GetComponent<DungemonController>().Exp += 45;
                Dungemon.GetComponent<DungemonController>().score += 75;
            }
            else
            {
                Dungemon.GetComponent<DungemonController>().Exp += 20;
                Dungemon.GetComponent<DungemonController>().score += 40;
            }
            Destroy(this.gameObject);
        }
    }

    public void GenerateItem()
    {
        int prop = Random.Range(0, 20);
        if (prop % 2 == 0)
            Instantiate(food, transform.position, Quaternion.identity);
        if (prop % 3 == 0)
            Instantiate(bomb, transform.position, Quaternion.identity);
        if (prop % 5 == 0)
            Instantiate(cool, transform.position, Quaternion.identity);
        else
            Instantiate(exp, transform.position, Quaternion.identity);

    }

}
