using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemy;
    public float attackRange;

    public int damage;

    public Animator camAnim;
    public Animator anim;

    void Update()
    {
      if(timeBtwAttack <= 0)
      {
            if (Input.GetButtonDown("Fire1"))
            {
                camAnim.SetTrigger("shake");
                //camAnim.SetBool("isShaking", true);
                anim.SetTrigger("Attacking");
                //anim.SetBool("isAttacking", true);
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);
                foreach(Collider2D enemy in enemiesToDamage)
                {
                    enemy.GetComponent<EnemyTest>().TakeDamage(damage);
                }
            }
                

            timeBtwAttack = startTimeBtwAttack;
      }
      else
      {
            timeBtwAttack -= Time.deltaTime;
      }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}
