using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed;
    public bool chase = false;
    public Transform startingPoint;
    private GameObject player;
    public float timer;
    private float intTimer;
    private bool attackMode;

    private Animator animator;

    public Transform attackPoint;

    [SerializeField] private float attackSpeed = 1f;
    private float canAttack;

    public float attackRange;
    public float attackDamage;
    public LayerMask playerLayers;

    private bool cooldown;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }
            
        if(chase == true)
        {
            Chase();
        }
        else
        {
            ReturnStartPoint();
        }

        Flip();
    }

    public void Attack_Cooldown()
    {
        StartCoroutine(Cooldown());
    }

    private void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if(Vector2.Distance(transform.position, player.transform.position) <= 0.5f)
        {
            //change speed, shoot, animation, attack
            timer = intTimer;
            attackMode = true;

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
        else
        {
            //reset variables
        }
    }

    private void ReturnStartPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, speed * Time.deltaTime);
    }

    private void Flip()
    {
        if (transform.position.x > player.transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 10);
        else
            transform.rotation = Quaternion.Euler(0, 180, 10);
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
