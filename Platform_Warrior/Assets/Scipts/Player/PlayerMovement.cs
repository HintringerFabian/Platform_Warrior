using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour {
    private float moveSpeed = 10f;
    private float jumpForce = 10f;
    //private float maxVelocity = 20f;
    private float moveX;

    private bool jumping = false;
    public bool runningLeft;
    public bool runningRight;

    private Rigidbody2D rigBod;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private const string WALK_ANIMATION = "Walk";
    private const string JUMP_ANIMATION = "Jump";
    private const string GROUND_TAG = "Ground";

    private void Awake() {
        rigBod = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        playerKeyboardInput();
        animateCharacter();
    }

    void FixedUpdate() {
        playerJump();
    }

    void playerKeyboardInput() {
        moveX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(moveX, 0f) * Time.deltaTime * moveSpeed;
    }

    void playerJump() {
        if( Input.GetButton("Jump") && jumping == false) {
            Debug.Log("Player should jump");
            rigBod.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumping = true;
            animator.SetTrigger(JUMP_ANIMATION);
        }

    }

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

    private void OnCollisionEnter2D(Collision2D collision) {
        if( collision.gameObject.CompareTag(GROUND_TAG) ) {
            jumping = false;
        }
    }
}
