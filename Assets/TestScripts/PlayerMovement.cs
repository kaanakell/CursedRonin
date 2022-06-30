using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float fCutJumpHeight = 0.5f;

    private float fHorizontalDamping = 0.5f;

    private Rigidbody2D rb;

    public bool facingRight = true;

    private bool isGrounded = false;
    private bool canJump = true;

    private float fJumpPressedRememberTime = 0.2f;
    private float fJumpPressedRemember = 0f;

    private float fGroundRememberTime = 0.2f;
    private float fGroundRemember = 0f;

    private bool canMidAirJump = true;
    private int midAirJump;
    private int midAirJumpValue = 1;

    [SerializeField] LayerMask groundLayer;

    public CapsuleCollider2D regularColl;

    [Header("For Dash")]
    [SerializeField]
    private float dashTime = 0.2f;
    public float dashSpeed;
    private bool isDashing;
    public float distanceBetweenImages;
    public float dashCoolDown;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;

    private float moveInput = 1;

    public ParticleSystem dust;
    public ParticleSystem dustWall;


    [Header("For WallSliding")]
    private bool isTouchingLeft, isTouchingRight, onWall, sliding;

    private bool canWallJump;
    private float gravity_temp;

    private bool canMove = true;
    private bool canWallJumpMovement = true;
    private bool canFlip;


    private Vector3 m_Velocity = Vector3.zero;

    public Animator animator;

    private BossHealth bosshp;

    //[Header("Corner Correction")]
    //[SerializeField] private float _topRaycastLenght;
    //[SerializeField] private Vector3 _edgeRaycastOffset;
    //[SerializeField] private Vector3 _innerRaycastOffset;
    //private bool _canCornerCorrect;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        gravity_temp = rb.gravityScale;

        
    }


    void FixedUpdate()
    {
        Movement();
        //CheckCollision();
        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }
        //if (_canCornerCorrect)
        //    CornerCorrect(rb.velocity.y);
        WallSlide();
    }

    private void Movement()
    {
        if(canMove == true && canWallJumpMovement)
        {
            moveInput = rb.velocity.x;
            moveInput += Input.GetAxis("Horizontal");
            moveInput *= Mathf.Pow(1f - fHorizontalDamping, Time.deltaTime * speed);
            rb.velocity = new Vector2(moveInput, rb.velocity.y);
            float animSpeed = Mathf.Abs(moveInput) > 1f ? Mathf.Abs(moveInput) : 0;
            animator.SetFloat("Speed", animSpeed);
            rb.sharedMaterial.friction = 0f;
        }
        
        
    }

    void Update()
    {
        Jump();
        Dash();
        CheckDash();
    }

    

    void Flip()
    {
        canFlip = true;
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        CreateDust();
    }


    public void Jump()
    {
        fGroundRemember -= Time.deltaTime;
        if (isGrounded)
        {
            fGroundRemember = fGroundRememberTime;
        }


        fJumpPressedRemember -= Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {
            fJumpPressedRemember = fJumpPressedRememberTime;
        }
        

        if (isGrounded == true)
        {
            midAirJump = midAirJumpValue;
            canJump = true;
        }

        if ((fJumpPressedRemember > 0) && (fGroundRemember > 0))
        {

            fGroundRemember = 0;
            fJumpPressedRemember = 0;
            animator.SetTrigger("Jumping");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            FindObjectOfType<AudioManager>().Play("PlayerJump");
            canMidAirJump = true;
            CreateDust();
        }

        if (Input.GetButtonUp("Jump"))
        {
            if(rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * fCutJumpHeight);
            }
        }

        if ((fJumpPressedRemember > 0) && (fGroundRemember < 0) && midAirJump > 0 && canMidAirJump == true)
        {
            rb.velocity = Vector2.up * jumpForce;
            animator.SetTrigger("Jumping");
            midAirJump--;
            FindObjectOfType<AudioManager>().Play("PlayerJump");
            CreateDust();
            if (midAirJump == 0)
            {
                canMidAirJump = false;
                canJump = false;
            }

        }


        isGrounded = Physics2D.Raycast(transform.position, -Vector2.up, 1.2f, groundLayer);
        animator.SetBool("Grounded", isGrounded);
    }

    public void Dash()
    {
        if (Input.GetButtonDown("Dash"))
        {
            if(Time.time >= (lastDash + dashCoolDown))
            AttemptToDash();
        }
    }

    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            if(dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2(dashSpeed * moveInput, rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos -= transform.position.x;
                }
            }

            if(dashTimeLeft <= 0 || onWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }
            
        }
    }

    public void WallSlide()
    {
        isTouchingLeft = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x - 0.4f, gameObject.transform.position.y + 0.2f), new Vector2(0.5f, 0.8f), 0f, groundLayer);
        isTouchingRight = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x + 0.4f, gameObject.transform.position.y + 0.2f), new Vector2(0.5f, 0.8f), 0f, groundLayer);

        onWall = isTouchingRight || isTouchingLeft;

        if (isGrounded || (!isTouchingLeft && !isTouchingRight))
        {
            if (sliding)
            {
                canFlip = true;
                canWallJumpMovement = true;
                sliding = false;
                canWallJump = false;
                rb.gravityScale = gravity_temp;
                animator.SetBool("isSliding", false);
                CreateDust_Wall();
            }
        }

        if (onWall && !isGrounded && rb.velocity.y < 0)
        {
            if (!sliding)
            {
                canFlip = false;
                rb.velocity = new Vector2(0, rb.velocity.y);
                rb.sharedMaterial.friction = 0f;
                canWallJumpMovement = false;
                sliding = true;
                rb.gravityScale = 0.2f;
                animator.SetTrigger("slide");
                animator.SetBool("isSliding", true);
                canWallJump=true;
                CreateDust_Wall();
            }
        }

        if (canWallJump == true && Input.GetButton("Jump"))
        {
            if (isTouchingLeft)
            {
                
                Debug.Log("Dokundu");
                rb.velocity = new Vector2(1.1f, 1.1f) * jumpForce;
                animator.SetTrigger("Jumping");
                StartCoroutine(Activate_Movement());

            }
            if (isTouchingRight)
            {

                Debug.Log("Dokundu");
                rb.velocity = new Vector2(-1.1f, 1.1f) * jumpForce;
                animator.SetTrigger("Jumping");
                StartCoroutine(Activate_Movement());
            }

        }
    }

    void CreateDust()
    {
        dust.Play();
    }

    void CreateDust_Wall()
    {
        if(sliding == true)
        {
            dustWall.Play();
        }
        else
        {
            dustWall.Stop();
        }
    }


    //void CornerCorrect(float Yvelocity)
    //{
    //    RaycastHit2D _hit = Physics2D.Raycast(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLenght, Vector3.left, _topRaycastLenght, groundLayer);
    //    if (_hit.collider != null)
    //    {
    //        float _newPos = Vector3.Distance(new Vector3(_hit.point.x, transform.position.y, 0f) + Vector3.up * _topRaycastLenght,
    //            transform.position - _edgeRaycastOffset + Vector3.up * _topRaycastLenght);
    //        transform.position = new Vector3(transform.position.x + _newPos, transform.position.y, transform.position.z);
    //        rb.velocity = new Vector2(rb.velocity.x, Yvelocity);
    //        return;
    //    }

    //    _hit = Physics2D.Raycast(transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLenght, Vector3.right, _topRaycastLenght, groundLayer);
    //    if(_hit.collider != null)
    //    {
    //        float _newpPos = Vector3.Distance(new Vector3(_hit.point.x, transform.position.y, 0f) + Vector3.up * _topRaycastLenght, transform.position + _edgeRaycastOffset + Vector3.up * _topRaycastLenght);
    //        transform.position = new Vector3(transform.position.x - _newpPos, transform.position.y, transform.position.z);
    //        rb.velocity = new Vector2(rb.velocity.x, Yvelocity);
    //    }

    //    _canCornerCorrect = Physics2D.Raycast(transform.position + _edgeRaycastOffset, Vector2.up, _topRaycastLenght, groundLayer) &&
    //        !Physics2D.Raycast(transform.position + _innerRaycastOffset, Vector2.up, _topRaycastLenght, groundLayer) ||
    //        Physics2D.Raycast(transform.position - _edgeRaycastOffset, Vector2.up, _topRaycastLenght, groundLayer) &&
    //        !Physics2D.Raycast(transform.position - _innerRaycastOffset, Vector2.up, _topRaycastLenght, groundLayer);
    //}

    IEnumerator Activate_Movement()
    {
        canMove = false;
        yield return new WaitForSeconds(0.1f);
        canWallJumpMovement = true;
        canMove = true;
        yield break;
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(transform.position, -Vector2.up * 1.2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x - 0.4f, gameObject.transform.position.y + 0.2f), new Vector2(0.5f, 0.8f));
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x + 0.4f, gameObject.transform.position.y + 0.2f), new Vector2(0.5f, 0.8f));

        ////Corner Check
        //Gizmos.DrawLine(transform.position + _edgeRaycastOffset, transform.position + _edgeRaycastOffset + Vector3.up * _topRaycastLenght);
        //Gizmos.DrawLine(transform.position - _edgeRaycastOffset, transform.position - _edgeRaycastOffset + Vector3.up * _topRaycastLenght);
        //Gizmos.DrawLine(transform.position + _innerRaycastOffset, transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLenght);
        //Gizmos.DrawLine(transform.position - _innerRaycastOffset, transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLenght);

        ////Corner Distance Check
        //Gizmos.DrawLine(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLenght,
        //    transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLenght + Vector3.left * _topRaycastLenght);
        //Gizmos.DrawLine(transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLenght,
        //    transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLenght + Vector3.left * _topRaycastLenght);



    }

}
