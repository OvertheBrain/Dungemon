using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    public int life = 50;

    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource Roar;

    private float attackCounter, attackTime = 3;

    public float dis;
    public GameObject Player;
    private bool FacceLeft = false;

    public GameObject Fireball, Blast;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Roar = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDir();
        attackCounter += Time.deltaTime;
        if (attackCounter >= attackTime)
        {
            Attack1();
            attackCounter = 0;
        }
        Vector3 distance = transform.position - Player.transform.position;
        dis = distance.magnitude;
        if (dis < 4 && Mathf.Abs(Player.transform.position.x-transform.position.x) < 3
            && Player.transform.position.y < transform.position.y)
            Attack2();
        Die();
    }

    void ChangeDir()
    {
        if(Player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
            FacceLeft = false;
        }
        else if(Player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            FacceLeft = true;
        }
    }

    void Attack1()
    {
        anim.SetTrigger("Attack1");
        for(int i=1; i<=3; i++)
        {
            int dir = (FacceLeft) ? -1 : 1;
            Instantiate(Fireball, transform.position + new Vector3(dir*1.5f, -1f, 0), Quaternion.identity).GetComponent<Fireball>().dir = i*dir;
        }
    }

    void Attack2()
    {
        Roar.Play();
        anim.SetBool("isBlast", true);
        anim.SetTrigger("Attack2");
        int dir = (FacceLeft) ? -1 : 1;
        Instantiate(Blast, transform.position+new Vector3(dir*2, -1.8f, 0), Quaternion.identity).GetComponent<lightball>().direction = dir;
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
            GameManager.instance.score += 2000;
            GameManager.instance.GameWin = true;
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LightBall"))
        {
            Destroy(other.gameObject);
            Hurt();
        }
        if (other.CompareTag("Explosion"))
        {
            if (other.gameObject.GetComponent<Bomb>().ignite)
            {
                Hurt();
                life -= 3;
            }
        }

    }
}
