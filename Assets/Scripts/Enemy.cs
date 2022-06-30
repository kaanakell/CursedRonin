using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    float currentHealth;


    public GameObject soulParticle;
    public GameObject sparkleEffect;

    public Animator animator;

    public Rigidbody2D rb;
    public float knockBackForce = 10f;
    public float knockBackForceUp = 2f;

    public LayerMask whatIsGround;

    public float knockTime;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        
    }

    // Update is called once per frame
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        
    }

    void Update()
    {
        

    }

    

    public void TakeDamage(float damage)
    {
        Debug.Log("Give Damage");
        Instantiate(sparkleEffect, transform.position, Quaternion.identity);
        currentHealth -= damage;
        FindObjectOfType<AudioManager>().Play("SwordHit");

        knockBack();

        //Play hurt anim
        animator.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void knockBack()
    {
        Transform attacker = getClosestDamageSource();
        Vector2 knockbackDirection = new Vector2(transform.position.x - attacker.transform.position.x, 0);
        rb.velocity = new Vector2(knockbackDirection.x, knockBackForceUp) * knockBackForce;
        StartCoroutine(KnockCo(rb));
    }

    public Transform getClosestDamageSource()
    {
        GameObject[] DamageSources = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        Transform currentClosestDamageSource = null;

        foreach(GameObject go in DamageSources)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, go.transform.position);
            if(currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                currentClosestDamageSource = go.transform;
            }
        }

        return currentClosestDamageSource;
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        FindObjectOfType<AudioManager>().Play("DeathSound");
        //Die anim
        animator.SetTrigger("IsDead");
        //Disable enemy
        //GetComponent<Collider2D>().enabled = false;
        Invoke("Destroy", 0.2f);
        this.enabled = false;
    }

    void Destroy()
    {
        Destroy(gameObject);
        Instantiate(soulParticle, transform.position, Quaternion.identity);
    }

    private IEnumerator KnockCo(Rigidbody2D rb)
    {
        if (rb != null)
        {
            yield return new WaitForSeconds(knockTime);
            rb.velocity = Vector2.zero;
        }
    }

}
