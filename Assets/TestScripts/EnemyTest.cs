using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public int health;
    public float speed;

    private Animator anim;
    public GameObject sparkleEffect;

    private bool isHit;

    public Rigidbody2D rb;
    public float knockBackForce = 10f;
    //public float knockBackForceUp = 2f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }


        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        //if(isHit == true)
        //{
        //    knockBack();
        //}

        Instantiate(sparkleEffect, transform.position, Quaternion.identity);
        health -= damage;
        Debug.Log("damage Taken");

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Blade")
        {
            Vector2 difference = (transform.position - other.transform.position).normalized;
            Vector2 force = difference * knockBackForce;
            rb.AddForce(force, ForceMode2D.Impulse); //if you don't want to take into consideration enemy's mass then use ForceMode.VelocityChange
        }
    }
}
