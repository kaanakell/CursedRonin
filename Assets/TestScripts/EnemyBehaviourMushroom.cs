using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourMushroom : MonoBehaviour
{
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject hotZone;
    public GameObject triggerArea;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask playerLayers;
    public float attackDamage;

    private Animator animator;
    private float distance;
    private bool attackMode;
    private bool cooling;
    private float intTimer;

    private bool cooldown;

    [SerializeField] private float attackSpeed = 1f;
    private float canAttack;

    private BodyAnim bd;

    void Awake()
    {
        SelectTarget();
        intTimer = timer;
        animator = GetComponentInChildren<Animator>();
        bd = GetComponentInChildren<BodyAnim>();
    }

    void Update()
    {
        if (!attackMode)
        {
            Move();
        }

        if (!InsideofLimit() && !inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("Mushroom_Attack"))
        {
            SelectTarget();
        }

        if (inRange)
        {
            EnemyLogic();
        }
    }

    public void Attack_Cooldown()
    {
        StartCoroutine(Cooldown());
    }

    public void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            StopAttack();
        }
        else if (attackDistance >= distance && !cooldown)
        {
            Attack();
        }

        //if (cooling)
        //{
        //    Cooldown();
        //    animator.SetBool("Attack", false);
        //}
    }

    public void Move()
    {
        animator.SetBool("canWalk", true);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Mushroom_Attack"))
        {
            Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, 10);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    public void Attack()
    {
        timer = intTimer;
        attackMode = true;

        animator.SetBool("canWalk", false);
        animator.SetBool("Attack", true);

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

        foreach (Collider2D player in hitPlayer)
        {
            if (player.GetComponent<PlayerHealth>() != null && attackSpeed <= canAttack)
            {
                player.GetComponent<PlayerHealth>().Hurt(attackDamage);
                canAttack = 0f;
            }
            else
            {
                canAttack += Time.deltaTime;
            }
        }
    }

    //public void Cooldown()
    //{
    //    if (!cooling)
    //        return;
    //    timer -= Time.deltaTime;

    //    if(timer <= 0 && cooling && attackMode)
    //    {
    //        cooling = false;
    //        timer = intTimer;
    //    }
    //}

    public void StopAttack()
    {
        cooling = false;
        attackMode = false;
        animator.SetBool("Attack", false);
    }



    //public void TriggerCooling()
    //{
    //    cooling = true;
    //}

    private bool InsideofLimit()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        Flip();
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;

        if (transform.position.x > target.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }

        transform.eulerAngles = rotation;
    }

    IEnumerator Cooldown()
    {
        cooldown = true;
        attackMode = false;
        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(timer);
        cooldown = false;
    }
}
