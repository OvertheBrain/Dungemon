using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungemonController : MonoBehaviour
{
    // Start is called before the first frame update
    public float Exp;
    public int level;
    public int life, maxlife;
    public float speed;
    public int ATK;
    public int score = 1000;

    private AudioSource died;

    private Vector2 movement;
    public int direction = 1;
    private bool isSkill = false;
    private bool alive = true;

    private float currentHurt, hurtTime = 0.8f;
    private float period = 0.8f, currentPeriod;
    private bool Burned = false;
    private float currentBurning = 0, BurningTime = 4.5f;
    private bool isCollide, onLeft, onRight;
    private Vector3 StartPos, EndPos;
    private bool faint;
    private float faintperiod, faintTime = 1.5f;

    public float CurrentCooldown1 = 6, CurrentCooldown2 = 6, CurrentCooldown3 = 6;
    public float cooldown1, cooldown2, cooldown3;

    public float dead = 16, deathCooldown = 15;

    public GameObject Lightball;

    private Animator anim;
    private Rigidbody2D rb;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        died = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (faint)
            Faint();
        else if (alive)
        {
            Grow();
            Die();
            Attack();
            CollideMove();
            cooldownUpdate();
            Skill();
            Run();
            GMupdate();
        }
        if (dead < deathCooldown)
        {
            dead += Time.deltaTime;
            
        }
        else if(dead >= deathCooldown)
        {
            alive = true;
            GameManager.instance.dun.status = Dungemon.states.Normal;
        }
    }

    private void GMupdate()
    {
        currentHurt += Time.deltaTime;
        if (Burned) GameManager.instance.dun.status = Dungemon.states.Burn;
        Burning();
        GameManager.instance.dun.life = life;
        GameManager.instance.dun.score = score;
        GameManager.instance.dun.level = level;
        GameManager.instance.dun.deathCooldown = deathCooldown;
        GameManager.instance.dun.dead = dead;
        GameManager.instance.dun.currentCooldown1 = CurrentCooldown1;
        GameManager.instance.dun.currentCooldown2 = CurrentCooldown2;
        GameManager.instance.dun.currentCooldown3 = CurrentCooldown3;
        GameManager.instance.dun.cooldown1 = cooldown1;
        GameManager.instance.dun.cooldown2 = cooldown2;
        GameManager.instance.dun.cooldown3 = cooldown3;
    }

    private void Grow()
    {       
        if (Exp < 1600)
            level = (int)(0.0125 * Exp + 1);
        else
            level = 20 + (int)(0.0125 * (Exp - 1600) / 1.5);

        if (level > 30) level = 30;

        ATK = level / 7 + 2;
        speed = 4 + level / 5;
        maxlife = 15 + level;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Enemy"))
        {
            if (anim.GetBool("isAttack"))
            {
                other.gameObject.GetComponent<EnemyController>().Hurt();
                other.gameObject.GetComponent<EnemyController>().life -= ATK - 1;
            }
            else
                Hurt();
        }
        if (other.CompareTag("Boss"))
        {
            if (anim.GetBool("isAttack"))
            {
                other.gameObject.GetComponent<GoblinController>().Hurt();
                other.gameObject.GetComponent<GoblinController>().life -= ATK - 1;
            }
            if (other.gameObject.GetComponent<GoblinController>().anim.GetBool("isAttack"))
            {
                Impact(other.gameObject);
                if (other.gameObject.name == "Mushroom(Clone)")
                    faint = true;
            }
            else
                Hurt();
        }
        if (other.CompareTag("Dragon"))
        {
            if (anim.GetBool("isAttack"))
            {
                other.gameObject.GetComponent<DragonController>().Hurt();
                other.gameObject.GetComponent<DragonController>().life -= ATK - 1;
            }
            else
                Hurt();
        }
        if (other.CompareTag("Attack"))
        {
            Destroy(other.gameObject);
            Hurt();
            life--;
        }
        if (other.CompareTag("Food"))
        {
            if (life >= maxlife) return;
            life += 3;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Exp"))
        {
            Exp += 25;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("FireBall"))
        {
            if (currentHurt >= hurtTime)
            {
                Hurt();
                if (Burned)
                    life -= 4;
                else
                    life -= 2;
                Burned = true;
                currentBurning = 0;
                currentHurt = 0;
            }
        }
        if (other.CompareTag("Blast"))
        {
            if (currentHurt >= hurtTime)
            {
                Impact(GameObject.FindGameObjectWithTag("Boss"));
                if (Burned)
                    life -= 1;
                Burned = true;
                currentBurning = 0;
                currentHurt = 0;
            }
        }

    }

    private void cooldownUpdate()
    {
        if (CurrentCooldown1 < cooldown1)
        {
            // 更新冷却
            CurrentCooldown1 += Time.deltaTime;
        }
        if (CurrentCooldown2 < cooldown2)
        {
            // 更新冷却
            CurrentCooldown2 += Time.deltaTime;
        }
        if (CurrentCooldown3 < cooldown3)
        {
            // 更新冷却
            CurrentCooldown3 += Time.deltaTime;
        }
    }

    private void Burning()
    {
        if (Burned)
        {
            speed = 5;
            currentBurning += Time.deltaTime;
            if (currentBurning >= BurningTime)
            {
                speed = 8;
                Burned = false;
                GameManager.instance.dun.status = Dungemon.states.Normal;
                currentBurning = 0;
            }
        }
    }

    void Skill()
    {

            if (Input.GetKeyDown(KeyCode.Alpha1) && !isSkill)
            {
                if (CurrentCooldown1 >= cooldown1)
                {
                    isSkill = true;
                    anim.SetTrigger("skill");
                    isSkill = false;
                    CurrentCooldown1 = 0;
                    transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                    if (life >= maxlife) return;
                    life ++;
            }

            }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !isSkill)
        {
            if (CurrentCooldown2 >= cooldown2)
            {
                isSkill = true;
                anim.SetTrigger("skill");
                Instantiate(Lightball, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity).GetComponent<lightball>().direction = direction;
                isSkill = false;
                CurrentCooldown2 = 0;
            }

        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && !isSkill)
        {
            if (CurrentCooldown3 >= cooldown3)
            {
                isSkill = true;
                anim.SetTrigger("skill");
                GameObject Player = GameObject.FindGameObjectWithTag("Player");
                Player.GetComponent<PlayerController>().life++;
                Player.GetComponent<PlayerController>().CurrentCooldown = 4 ;
                isSkill = false;
                CurrentCooldown3 = 0;
            }

        }
    }

    void Run()
    {
        if (!isSkill)
        {
            //anim.SetBool("isAttack", false);

            if (Input.GetKey(KeyCode.T))
            {
                movement.y = 1;
                anim.SetBool("isRun", true);
            }
            else if (Input.GetKey(KeyCode.G))//按键盘S向下移动
            {
                movement.y = -1;
                anim.SetBool("isRun", true);
            }

            if (Input.GetKey(KeyCode.F))//按键盘A向左移动
            {
                direction = -1;
                movement.x = -1;

                transform.localScale = new Vector3(direction, 1, 1);
                anim.SetBool("isRun", true);
            }
            else if (Input.GetKey(KeyCode.H))//按键盘D向右移动
            {
                direction = 1;
                movement.x = 1;
                transform.localScale = new Vector3(direction, 1, 1);
                anim.SetBool("isRun", true);
            }
            else if (Input.GetKey(KeyCode.Space))//按键空格停止移动
            {
                movement = Vector2.zero;
                anim.SetBool("isRun", false);
            }

        }
    }

    void Attack()
    {
        if (anim.GetBool("isAttack"))
            currentPeriod += Time.deltaTime;
        if (currentPeriod >= period)
        {
            anim.SetBool("isAttack", false);
            currentPeriod = 0;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetBool("isAttack", true);
            anim.SetTrigger("attack");
        }

    }
    void Hurt()
    {
        if (currentHurt >= hurtTime)
        {
            anim.SetTrigger("hurt");
            if (direction == 1)
                rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
            else
                rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);

            life--;
            currentHurt = 0;
        }
    }

    public void Faint()
    {
        if (faint)
        {
            GameManager.instance.dun.status = Dungemon.states.Faint;
            faintperiod += Time.deltaTime;
        }
        if (faintperiod >= faintTime)
        {
            faintperiod = 0;
            faint = false;
            GameManager.instance.dun.status = Dungemon.states.Normal;
        }
    }

    public void Impact(GameObject boss)
    {
        Hurt();
        life--;
        isCollide = true;
        if (transform.position.x < boss.transform.position.x)
        {
            onRight = true;
            EndPos = new Vector3(transform.position.x - 1.5f, transform.position.y - 1, transform.position.z);
        }
        else
        {
            onLeft = true;
            EndPos = new Vector3(transform.position.x + 1.5f, transform.position.y - 1, transform.position.z);
        }
        StartPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

    }

    private void CollideMove()
    {
        if (isCollide)
        {
            if (onRight)
            {
                rb.position = Vector3.MoveTowards(transform.position,
        new Vector3(EndPos.x, EndPos.y, EndPos.z),
        10 * Time.deltaTime);
            }
            if (onLeft)
            {
                rb.position = Vector3.MoveTowards(transform.position,
        new Vector3(EndPos.x, EndPos.y, EndPos.z),
        10 * Time.deltaTime);
            }

            if (transform.position.x == EndPos.x)
                isCollide = false;
        }
    }
    void Die()
    {
        if (life <= 0)
        {
            isSkill = false;
            anim.SetTrigger("die");
            anim.SetBool("isRun", false);
            died.Play();
            score /= 2;           
            GameManager.instance.dun.status = Dungemon.states.Dead;
            life = maxlife;
            alive = false;
            dead = 0;
            
        }        
    }

    private void FixedUpdate()
    {
        if (!isSkill)
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);        
    }
}
