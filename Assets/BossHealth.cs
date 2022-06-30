using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossHealth : MonoBehaviour
{
    public float health = 1000f;
    public Slider healthBar;

    public GameObject SceneTransition;
    //public GameObject deathEffect;

    public bool isInvulnerable = false;


    private Animator animator;

    void Start()
    {
        SceneTransition.SetActive(false);
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        healthBar.value = health;  
    }

    public void TakeDamage(float attackDamage)
    {
        if (isInvulnerable)
            return;

        health -= attackDamage;
        FindObjectOfType<AudioManager>().Play("SwordHit");


        if (health <= 1000f)
        {
            animator.SetBool("IsEnraged", true);     
        }

        if (health <= 0f)
        {
            animator.SetBool("isDead", true);
            FindObjectOfType<AudioManager>().Play("BossDeath");
            SceneTransition.SetActive(true);
            Invoke("Die", 1f);
        }
    }

    void Die()
    {
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
