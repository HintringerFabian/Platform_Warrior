using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour {
    private float moveSpeed = 10f,
       jumpForce = 12f,
       moveX;

    public bool jumping = false,
        runningLeft,
        runningRight;

    private Vector3 playerInitialRotation;

    private Rigidbody2D rigBod;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private const string WALK_ANIMATION = "Walk",
        JUMP_ANIMATION = "Jump",
        PLATTFORM_TAG = "Plattform",
        GROUND_TAG = "Ground";

    // Start is called before the first frame update
    void Start() {
        rigBod = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInitialRotation = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update() {
        playerKeyboardInput();
        playerJump();
        animateCharacter();
        // do not rotate the player when jumping against edges,...
        transform.eulerAngles = playerInitialRotation;
    }

    // fixedUpdate will be called every 0.02 seconds
    void FixedUpdate() {
    }

    // get input for running
    void playerKeyboardInput() {
        moveX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(moveX, 0f) * Time.deltaTime * moveSpeed;
    }

    // get input for jumping
    void playerJump() {
        if( Input.GetButtonDown("Jump") && jumping == false) {
            rigBod.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumping = true;
            animator.SetTrigger(JUMP_ANIMATION);
        }
    }

    // animation for the movement
    private void animateCharacter() {
        if( moveX < 0 ) {
            // running left side
            animator.SetBool(WALK_ANIMATION, true);
            spriteRenderer.flipX = true;
            runningLeft = true;
            runningRight = false;
        } else if( moveX > 0 ) {
            // running right side
            animator.SetBool(WALK_ANIMATION, true);
            spriteRenderer.flipX = false;
            runningLeft = false;
            runningRight = true;
        } else {
            // standing still
            animator.SetBool(WALK_ANIMATION, false);
            runningLeft = false;
            runningRight = false;
        }
    }


    // when landing back on the ground set the bool jumping to false
    // then the character will be able to jump again
    private void OnCollisionEnter2D(Collision2D collision) {
        if( collision.gameObject.CompareTag(GROUND_TAG) || collision.gameObject.CompareTag(PLATTFORM_TAG) ) {
            jumping = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        jumping = true;
        // create falling animation + think of falling code when player is falling
    }
}
