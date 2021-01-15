using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDamageable
{
    private Rigidbody2D rb;
    private Animator anim;
    private FixedJoystick joystick;

    public float speed;
    public float jumpForce;

    [Header("Player State")]
    public float health;
    public bool isDead;
    public bool isHurt;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;

    [Header("States Check")]
    public bool isGround;
    public bool isJump;
    public bool canJump;

    [Header("Jump FX")]
    public GameObject jumpFX;
    public GameObject landFX;

    [Header("Attack Setting")]
    public GameObject bombPrefab;
    public float nextAttack = 0;
    public float attackRate;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        joystick = FindObjectOfType<FixedJoystick>();

        GameManager.instance.IsPlayer(this);

        health = GameManager.instance.LoadHealth();
        UIManager.instance.UpdateHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("dead", isDead);
        if (isDead)
            return;

        isHurt = anim.GetAnimatorTransitionInfo(1).IsName("player_hit");
        CheckInput();
    }

    public void FixedUpdate()
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        PhyscisCheck();
        //非受伤状态才可移动和跳跃
        if (!isHurt)
        {
            Movement();
            Jump();
        }
        
    }
    void CheckInput()
    {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            canJump = true;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Attack();
        }
    }
    private void Movement()
    {
        //键盘
        //float horizontalInput = Input.GetAxis("Horizontal");//-1~1 包含小数
        //float horizontalInput = Input.GetAxisRaw("Horizontal");//-1~1 不包含小数
        //操纵杆
        float horizontalInput = joystick.Horizontal;

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        //if(horizontalInput != 0)
        //{
        //    transform.localScale = new Vector3(horizontalInput, 1, 1);
        //}
        if (horizontalInput > 0)
        {
            //transform.localScale = new Vector3(1, 1, 1);
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (horizontalInput < 0)
        {
            //transform.localScale = new Vector3(-1, 1, 1);
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

    }
    void Jump()
    {
        if (canJump)
        {
            isJump = true;
            jumpFX.SetActive(true);
            jumpFX.transform.position = transform.position + new Vector3(0, -0.5f, 0);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canJump = false;
        }
    }
    public void ButtonJump()
    {
        if (isGround)
        {
            canJump = true;
        }
    }
    public void Attack()
    {
        if (Time.time > nextAttack)
        {
            Instantiate(bombPrefab, transform.position, bombPrefab.transform.rotation);

            nextAttack = Time.time + attackRate;
        }
    }
    public void PhyscisCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (!isGround)
        {
            rb.gravityScale = 4;
        }
        else
        {
            rb.gravityScale = 1;
            isJump = false;
        }
    }
    public void LandFX()//Animation Event
    {
        landFX.SetActive(true);
        landFX.transform.position = transform.position + new Vector3(0, -0.8f, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

    public void GetHit(float damage)
    {
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("player_hit"))
        {
            health -= damage;
            if (health < 1)
            {
                health = 0;
                isDead = true;
            }
            anim.SetTrigger("hit");
        }
        UIManager.instance.UpdateHealth(health);
    }
}
