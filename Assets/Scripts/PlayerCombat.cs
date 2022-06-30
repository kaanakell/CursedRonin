using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;

    public int combo;
    public bool isAttacking;
    //public bool canDeflect;
   

    public LayerMask enemyLayers;

    public float attackDamage = 40f;

    public float attackRate = 1.5f;
    float nextAttackTime = 0f;

    private bool facingRight;

    void Start()
    {
        animator = GetComponent<Animator>();   
    }




    // Update is called once per frame
    void Update()
    { 
        Combos();
    }

    void Attack()
    {
        //Play animation
        
        //Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Detect boss in range
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<Enemy>() != null)
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }
        //Damage Boss
        foreach (Collider2D boss in hitBoss)
        {
            if (boss.GetComponent<BossHealth>() != null)
            {
                boss.GetComponent<BossHealth>().TakeDamage(attackDamage);
            }
        }
    }

    //void Deflect()
    //{

    //}

    public void Combos()
    {
       if (Input.GetButton("Fire1") && !isAttacking)
       {
          isAttacking = true;
          animator.SetTrigger("" + combo);
          CinemachineShake.Instance.ShakeCamera(1.5f, .1f);
          FindObjectOfType<AudioManager>().Play("PlayerSwing");
          Attack();
          nextAttackTime = Time.time + 1f / attackRate;
       }
    }

    public void StartCombo()
    {
        isAttacking = false;
        if(combo < 2)
        {
            combo++;
        }
    }

    public void FinishAnim()
    {
        isAttacking=false;
        combo = 0;
    }



    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        
    }
}
