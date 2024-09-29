using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private Vector2 moveInput;
    public bool IsMoving { get; private set; }
    public bool OnIce { private get; set; }

    public int score = 0;

    
    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 LastCheckpoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        LastCheckpoint = transform.position;
    }

    private void FixedUpdate()
    {
        var grounded = IsGrounded();
        
        rb.velocity = new Vector2(moveInput.x * walkSpeed, rb.velocity.y);
        
        anim.SetBool("Grounded", grounded);
        anim.SetBool("Moving", IsMoving);
        anim.SetFloat("VelocityY", rb.velocity.y);

        // Set direction of player
        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // If player is below the screen, reset the level
        if (transform.position.y < -10)
        {
            // respawn player
            transform.position = LastCheckpoint;
        }
    }

    private bool IsGrounded()
    {
        // Check if player is grounded
        
        var pos = transform.position;
        var hit = Physics2D.Raycast(pos, Vector2.down, 1.1f);
        Debug.DrawRay(pos, Vector2.down * 1.1f, Color.red);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (OnIce)
        {
            moveInput *= 0.5f;
        }
        
        IsMoving = moveInput != Vector2.zero;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        var grounded = IsGrounded();
        if (grounded) {
            if (OnIce) {
                rb.AddForce(Vector2.up * (jumpForce * 0.8f), ForceMode2D.Impulse);
            } else {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        } 
    }

    public void Victory()
    {
        Debug.Log("Victory!");
        LastCheckpoint = transform.position;
        anim.SetTrigger("Victory");
    }
}
