using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator Anim;
    Rigidbody2D RigBody2D;
    float speed = 8f;
    float jumpSpeed = 8f;

    bool isGrounded = false;
    float groundCheckRadius = 0.2f;

    public Transform groundCheck;
    public LayerMask GroundType;

    void Start()
    {
        RigBody2D = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Anim.GetBool("TakeDamage") == false)
        {
            float InputMove = Input.GetAxis("Horizontal");

            if (InputMove < 0)
                transform.eulerAngles = new Vector3(0, 180, 0);

            if (InputMove > 0)
                transform.eulerAngles = new Vector3(0, 0, 0);

            if (InputMove == 0)
            {
                Anim.SetBool("IsWalking", false);
                Anim.SetBool("IsRunning", false);
            } 

            if (Input.GetKeyDown(KeyCode.E) && InputMove == 0)
            {
                Anim.SetBool("TakeDamage", true);
            }

            if (Input.GetKeyDown(KeyCode.W) && isGrounded || Input.GetKeyDown(KeyCode.Space) && isGrounded)
                Jump();

            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 12f;
            }
            else if (!Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = 8f;
            }
        }
    }
        
    private void FixedUpdate()
    {
        MovePlayer();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, GroundType);

        if (isGrounded == true)
        {
            Anim.SetBool("HasJumped", false);
        }
        else if (isGrounded == false)
        {
            Anim.SetBool("HasJumped", true);
            Anim.SetBool("IsWalking", false);
            Anim.SetBool("IsRunning", false);
        }
    }

    void MovePlayer()
    {
        if (Anim.GetBool("TakeDamage") == false)
        {
            RigBody2D.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, RigBody2D.velocity.y);
            if (Anim.GetBool("HasJumped") == false)
            {
                if (speed > 10)
                {
                    Anim.SetBool("IsRunning", true);
                    Anim.SetBool("IsWalking", false);
                }
                else if (speed < 10)
                {
                    Anim.SetBool("IsWalking", true);
                    Anim.SetBool("IsRunning", false);
                }
            }
        }          
    }

    public void setDamageFalse()
    {
        Anim.SetBool("TakeDamage", false);
    }

    private void Jump()
    {
        RigBody2D.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
    }
}
