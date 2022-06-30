using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public HealthBar healthBar;

    private Animator animator;

    public float damageOverTime;


    // Update is called once per frame
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        animator = GetComponent<Animator>();
        InvokeRepeating("DamageOverTime", 0, 1f);
    }

    private void Update()
    {
       
    }




    private void DamageOverTime()
    {
        Debug.Log("Damage");
        currentHealth -= damageOverTime;

        healthBar.SetHealth(currentHealth);

        //Play hurt anim
        //animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            animator.SetTrigger("IsDead");
            Invoke("Die", 1f);
        }
        else
        {
            Time.timeScale = 1;
        }

    }

    public void Hurt(float damage)
    {
        Debug.Log("Damage");
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        //Play hurt anim
        //animator.SetTrigger("HurtEnemy");

        if (currentHealth <= 0)
        {
            animator.SetBool("IsDead", true);
            Invoke("Die", 1f);
        }
        else
        {
            Time.timeScale = 1;
        }

    }

    public void BossHurt(float attackDamage)
    {
        Debug.Log("Damage");
        currentHealth -= attackDamage;

        healthBar.SetHealth(currentHealth);

        //Play hurt anim
        //animator.SetTrigger("HurtEnemy");

        if (currentHealth <= 0)
        {
            animator.SetBool("IsDead", true);
            Invoke("Die", 1f);
        }
        else
        {
            Time.timeScale = 1;
        }

    }

    public void BossEnrageHurt(float enragedAttackDamage)
    {
        Debug.Log("Damage");
        currentHealth -= enragedAttackDamage;

        healthBar.SetHealth(currentHealth);

        //Play hurt anim
        //animator.SetTrigger("HurtEnemy");

        if (currentHealth <= 0)
        {
            animator.SetBool("IsDead", true);
            Invoke("Die", 1f);
        }
        else
        {
            Time.timeScale = 1;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag.Equals("Bullet"))
        //{
        //    //animator.SetTrigger("Boom");
        //    currentHealth -= 5;
        //    Destroy(collision.gameObject);
        //}

        if (collision.gameObject.tag.Equals("Void"))
        {
            currentHealth -= 1000;
            gameObject.SetActive(false);
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            currentHealth -= 10;
            Destroy(collision.gameObject);
        }
    }*/

    void Die()
    {
        Debug.Log("Player died!");
        FindObjectOfType<AudioManager>().Play("DeathSound");
        //Time.timeScale = 0;
        //Die anim
        //GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
