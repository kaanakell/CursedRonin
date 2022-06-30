using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{

    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    private Vector3 m_Velocity = Vector3.zero;

    public Animator animator;

    public float speed;
    public float jumpForce;

    private float moveInput;

    private Rigidbody2D rb;

    private bool facingRight = true;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private int extraJumps;
    public int extraJumpsValue;

    public BoxCollider2D regularColl;
    public BoxCollider2D slideColl;

    public float maxSlideTime = 1.5f;
    public float slideSpeed = 5f;
    float slideTimer = 0f;

    bool slide = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();   
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);


        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }else if(facingRight == true && moveInput < 0)
        {
            Flip();
        }

        Move(moveInput * Time.deltaTime);
    }

    void Update()
    {
        if(isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }

        if (Input.GetButtonDown("Jump") && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        } else if(Input.GetButtonDown("Jump") && extraJumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        if(isGrounded == true)
        {
            animator.SetBool("IsJumping", false);
        }
        else
        {
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Crouch") && !slide)
        {
            slideTimer = 0f;

            regularColl.enabled = false;
            slideColl.enabled = true;

            slide = true;
        }

        if(slide == true)
        {
            animator.SetBool("IsSlide", true);
            animator.SetBool("IsJumping", false);
        }

        if (slide)
        {
            slideTimer += Time.deltaTime;

            if (slideTimer > maxSlideTime)
            {
                slide = false;

                regularColl.enabled = true;
                slideColl.enabled = false;

                animator.SetBool("IsSlide", false);
            }
        }

        if (slide == true)
        {
            speed += (slideSpeed + speed) * Time.deltaTime;
        }
        else if (slide == false)
        {
            speed = 10f;
        }

        

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void Move(float move)
    {           
            Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
            
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }
}
