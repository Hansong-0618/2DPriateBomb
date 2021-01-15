using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyBaseState currentState;

    public Animator anim;
    public int animState;
    private GameObject alarmSign;

    [Header("Base State")]
    public float health;
    public bool isDead;
    public bool hasBomb;
    public bool isBoss;

    [Header("Movement")]
    public float speed;
    public Transform pointA, pointB;
    public Transform targetPoint;

    [Header("Attack Setting")]
    public float attackRate;
    public float attackRange, skillRange;
    private float nextAttack = 0;

    public List<Transform> attackObj = new List<Transform>();

    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();

    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        alarmSign = transform.GetChild(0).gameObject;

    }
    private void Awake()
    {
        Init();
    }
    void Start()
    {
        TransitionToState(patrolState);
        if (isBoss)
            UIManager.instance.SetBossHealth(health);

        GameManager.instance.IsEnemy(this);

    }

    public virtual void Update()
    {
        if (isBoss)
            UIManager.instance.UpdateBossHealth(health);

        anim.SetBool("dead", isDead);
        if (isDead)
        {
            GameManager.instance.RemoveEnemy(this);
            return;
        }
        currentState.OnState(this);
        anim.SetInteger("state", animState);

        
    }

    public void TransitionToState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        FilpDriection();
    }

    public void FilpDriection()//NPC翻转
    {
        if (transform.position.x < targetPoint.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    public void SwitchPoint()//选择目标点
    {
        if (Mathf.Abs(pointA.position.x - transform.position.x) < Mathf.Abs(pointB.position.x - transform.position.x))
        {
            targetPoint = pointB;
        }
        else
        {
            targetPoint = pointA;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!attackObj.Contains(collision.transform) && !hasBomb && !isDead && !GameManager.instance.gameOver) 
        {
            attackObj.Add(collision.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        attackObj.Remove(collision.transform);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead && !GameManager.instance.gameOver)
        {
            StartCoroutine(OnAlerm());
        }
    }
    IEnumerator OnAlerm()
    {
        alarmSign.SetActive(true);
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSign.SetActive(false);
    }

    public void AttackAction()  //普通攻击
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < attackRange)
        {
            if (Time.time > nextAttack)
            {
                //播放攻击动画
                anim.SetTrigger("attack");
                Debug.Log("普通攻击！");
                nextAttack = Time.time + attackRate;
            }
        }
    }
    public virtual void SkillAction()   //技能攻击，每个不同的敌人有不同的攻击方式
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < attackRange)
        {
            if (Time.time > nextAttack)
            {
                //播放攻击动画
                anim.SetTrigger("skill");
                Debug.Log("使用技能！");
                nextAttack = Time.time + attackRate;
            }
        }
    }
}
