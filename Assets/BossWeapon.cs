using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    public int attackDamage = 20;
    public int enragedAttackDamage = 40;

    public Vector3 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;

    private bool cooldown;
    private bool attackMode;
    public float timer;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();  
    }

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D collInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);

        if(collInfo != null)
        {
            collInfo.GetComponent<PlayerHealth>().BossHurt(attackDamage);
            FindObjectOfType<AudioManager>().Play("BossAttackOne");

        }
    }

    public void EnragedAttack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D collInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);

        if (collInfo != null)
        {
            collInfo.GetComponent<PlayerHealth>().Hurt(enragedAttackDamage);
            FindObjectOfType<AudioManager>().Play("BossAttackTwo");

        }
    }

    public void Attack_Cooldown()
    {
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        cooldown = true;
        attackMode = false;
        animator.SetBool("canAttack", false);
        animator.SetBool("Idle", true);
        yield return new WaitForSeconds(timer);
        cooldown = false;
    }
}
