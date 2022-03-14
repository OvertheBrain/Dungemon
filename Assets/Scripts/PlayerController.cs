using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float KickBoardMovePower = 15f;
    private float scale = 0.4f;
    public GameObject lightball;
    public float cooldown;
    public float CurrentCooldown = 3;

    private Rigidbody2D rb;
    private Animator anim;
    public AudioSource pickup, cool, beat;
    Vector2 movement;
    public int direction = 1;
    private bool isSkill = false;

    private bool isCollide, onLeft, onRight;
    private Vector3 StartPos, EndPos;
    public bool faint;
    private float faintperiod, faintTime = 1.5f;

    private bool alive = true;
    public int life;
    public float speed;
    private float currentHurt, hurtTime = 0.8f;
    private float period = 0.8f, currentPeriod;

    private float currentGem, GemTime = 3.5f;
    private int bombnum = 0;
    public GameObject bomb;

    private bool Burned = false;
    private float currentBurning = 0, BurningTime = 4.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameManager.instance.coolDown = cooldown;
    }

    private void Update()
    {
        Restart();
        if (faint)
            Faint();
        else if (alive)
        {                       
            Die();
            Attack();
            CollideMove();
            if (CurrentCooldown < cooldown)
            {
                // 更新冷却
                CurrentCooldown += Time.deltaTime;
            }
            Skill();
            Run();
            SetBomb();
            GMupdate();
        }
    }

    private void GMupdate()
    {
        GameManager.instance.life = life;       
        GameManager.instance.CurrentCooldown = CurrentCooldown;
        currentHurt += Time.deltaTime;
        if (Burned) GameManager.instance.status = GameManager.states.Burn;
        CoolGem();
        Burning();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Enemy"))
        {
            if (anim.GetBool("isAttack"))
                other.gameObject.GetComponent<EnemyController>().Hurt();
            if (anim.GetBool("isKickBoard"))
            {
                other.gameObject.GetComponent<EnemyController>().KickboardCollide(this.gameObject);
                isSkill = false;
                anim.SetBool("isKickBoard", false);
                CurrentCooldown = 0;
            }
            else
                Hurt();
        }
        if (other.CompareTag("Boss"))
        {
            if (anim.GetBool("isAttack"))
                other.gameObject.GetComponent<GoblinController>().Hurt();
            if (anim.GetBool("isKickBoard"))
            {
                other.gameObject.GetComponent<GoblinController>().Hurt();
                other.gameObject.GetComponent<GoblinController>().life--;
                isSkill = false;
                anim.SetBool("isKickBoard", false);
                CurrentCooldown = 0;
            }
            if (other.gameObject.GetComponent<GoblinController>().anim.GetBool("isAttack"))
            {
                Impact(other.gameObject);
                beat.Play();
                if (other.gameObject.name == "Mushroom(Clone)")
                    faint = true;
            }
            else
                Hurt();
        }
        if (other.CompareTag("Dragon"))
        {
            if (anim.GetBool("isAttack"))
                other.gameObject.GetComponent<DragonController>().Hurt();
            if (anim.GetBool("isKickBoard"))
            {
                other.gameObject.GetComponent<DragonController>().Hurt();
                other.gameObject.GetComponent<DragonController>().life--;
                isSkill = false;
                anim.SetBool("isKickBoard", false);
                CurrentCooldown = 0;
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
            if (life >= 20) return;
            life += 3;
            pickup.Play();
            Destroy(other.gameObject);
;       }
        if (other.CompareTag("CoolGem"))
        {
            cooldown = 0;
            cool.Play();
            Destroy(other.gameObject);
            currentGem = 0;
            GameManager.instance.status = GameManager.states.Cool;
        }
        if (other.CompareTag("Bomb"))
        {
            if (other.gameObject.GetComponent<Bomb>().ignite) return;
            bombnum++;
            pickup.Play();
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

    private void Burning()
    {
        if (Burned)
        {
            speed = 5;
            currentBurning += Time.deltaTime;
            if(currentBurning >= BurningTime)
            {
                speed = 8;
                Burned = false;
                GameManager.instance.status = GameManager.states.Normal;
                currentBurning = 0;
            }
        }
    }

    void Skill()
    {
        if (GameManager.instance.gender == 0)
        {
            if (CurrentCooldown >= cooldown)
            {
                if (Input.GetKeyDown(KeyCode.Alpha4) && isSkill)
                {
                    isSkill = false;
                    anim.SetBool("isKickBoard", false);
                    CurrentCooldown = 0;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4) && !isSkill)
                {
                    isSkill = true;
                    anim.SetBool("isKickBoard", true);                   
                }     
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha4) && !isSkill)
            {
                if (CurrentCooldown >= cooldown)
                {
                    isSkill = true;
                    anim.SetBool("isAttack", true);
                    anim.SetTrigger("attack");
                    Instantiate(lightball, transform.position + new Vector3(0, 0.75f, 0), Quaternion.identity).GetComponent<lightball>().direction = direction;
                    isSkill = false;
                    CurrentCooldown = 0;
                }
                
            }
        }

    }

    void Run()
    {
        if (!isSkill)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            Vector3 moveVelocity = Vector3.zero;
            anim.SetBool("isRun", false);
            //anim.SetBool("isAttack", false);


            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                direction = -1;
                moveVelocity = Vector3.left;

                transform.localScale = new Vector3(direction, 1, 1)*scale;
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);

            }
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                direction = 1;
                moveVelocity = Vector3.right;

                transform.localScale = new Vector3(direction, 1, 1)*scale;
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);
            }
            if (Input.GetAxisRaw("Vertical")!=0)
            {
                anim.SetBool("isRun", true);
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
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
            GameManager.instance.status = GameManager.states.Faint;
            faintperiod += Time.deltaTime;
        }
        if(faintperiod >= faintTime)
        {
            faintperiod = 0;
            faint = false;
            GameManager.instance.status = GameManager.states.Normal;
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
            EndPos = new Vector3(transform.position.x - 1.5f, transform.position.y-1, transform.position.z);
        }
        else
        {
            onLeft = true;
            EndPos = new Vector3(transform.position.x + 1.5f, transform.position.y-1, transform.position.z);
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
            anim.SetBool("isKickBoard", false);
            anim.SetTrigger("die");
            alive = false;
            GameManager.instance.status = GameManager.states.Dead;
            GameManager.instance.GameOver = true;
        }
    }
    void Restart()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            isSkill = false;
            anim.SetBool("isKickBoard", false);
            anim.SetTrigger("idle");
            alive = true;
        }
    }

    private void FixedUpdate()
    {
        if(!isSkill)
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        else if(GameManager.instance.gender == 0)
        {
            Vector2 moveVelocity = Vector2.zero;
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                direction = -1;
                moveVelocity = Vector2.left;

                transform.localScale = new Vector3(direction, 1, 1) * scale;
            }
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                direction = 1;
                moveVelocity = Vector2.right;

                transform.localScale = new Vector3(direction, 1, 1) * scale;
            }
            //transform.position += moveVelocity * KickBoardMovePower * Time.deltaTime;
            rb.MovePosition(rb.position + moveVelocity * KickBoardMovePower * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            if (isSkill)
            {
                isSkill = false;
                anim.SetBool("isKickBoard", false);
                CurrentCooldown = 0;
            }
        }
    }

    private void CoolGem()
    {
        if (currentGem >= 7) return;
        currentGem += Time.deltaTime;
        if(currentGem >= GemTime)
        {
            cooldown = (GameManager.instance.gender==0)?3:2;
            GameManager.instance.status = GameManager.states.Normal;
        }
    }

    private void SetBomb() 
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(bombnum > 0)
            {
                Instantiate(bomb, transform.position, Quaternion.identity).GetComponent<Bomb>().ignite = true;
                bombnum--;
            }
        }
    }

}
