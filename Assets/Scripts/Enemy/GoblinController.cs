using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
    readonly Vector2[] directions =
    {
        Vector2.zero,
        Vector2.left,
        Vector2.right,
        Vector2.up,
        Vector2.down
    };
    private double leftpoint, rightpoint, uppoint, downpoint;
    private bool FaceLeft = true;
    private int CurrentDir = 0;
    private float PassedTime = 2;
    public float idletime = 0;

    public int life = 30;
    public float speed = 7f;

    public Animator anim;
    private Rigidbody2D rb;
    private AudioSource defeat;

    private float attackCounter, attackTime = 3;
    private float period = 1f, currentPeriod;

    public GameObject arrow;
    public GameObject food, cool, exp, bomb;

   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        defeat = GetComponent<AudioSource>();
        leftpoint = transform.position.x - 3.1;
        rightpoint = transform.position.x + 3.1;
        uppoint = transform.position.y + 2.5;
        downpoint = transform.position.y - 2.5;
        StartCoroutine(Die());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        attackCounter += Time.deltaTime;
        if(attackCounter >= attackTime)
        {
            int num = Random.Range(0, 2);

            if (num == 0) Attack1();

            else Attack2();

            attackCounter = 0;
        }
        if (anim.GetBool("isAttack"))
            currentPeriod += Time.deltaTime;
        if (currentPeriod >= period)
        {
            anim.SetBool("isAttack", false);
            currentPeriod = 0;
        }
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
        if (CurrentDir > 0)
            anim.SetBool("isMove", true);

        if (dir.x == 1)
        {
            FaceLeft = false;
        }
        else if (dir.x == -1)
        {
            FaceLeft = true;
        }

        if (FaceLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (transform.position.x < leftpoint)
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


    public void Hurt()
    {
        anim.SetTrigger("Hurt");
        life--;
    }

    void Attack1()
    {
            anim.SetBool("isAttack", true);
            anim.SetTrigger("Attack1");
    }

    void Attack2()
    {
            
            anim.SetBool("isAttack", true);
            anim.SetTrigger("Attack2");
        if (this.gameObject.name == "Eye(Clone)")
        {
            for(int i=0; i<4; i++)
            {
                Instantiate(arrow, transform.position, Quaternion.identity).GetComponent<arrow>().direction = i;
            }
        }
        if(this.gameObject.name == "Skeleton(Clone)")
        {
            if (life >= 30) return;
            life++;
        }
    }

    IEnumerator Die()
    {
        while (true)
        {
            if (life <= 0)
            {
                anim.SetTrigger("Die");
                defeat.Play();
                GameManager.instance.score += 1000;
                GameManager.instance.BossDefeat++;
                int prop = Random.Range(0, 20);
                if (prop <= 15)
                    GenerateItem();
                yield return new WaitForSeconds(1.5f);
                Destroy(this.gameObject);
            }
            yield return null;
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
        if (other.CompareTag("Bomb"))
        {
            Hurt();
            life -= 2;
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
