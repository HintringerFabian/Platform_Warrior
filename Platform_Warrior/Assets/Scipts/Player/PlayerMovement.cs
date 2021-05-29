using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour {
    private float moveSpeed = 10f,
        jumpForce = 12f,
        moveX,
        moveY,
        oldPlayerPosY;

    public bool isJumping = false,
        jumpInput = false,
        isFalling = false,
        jumpAnimPlaying,
        initGroundTouch = false,
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
    private void Start() {
        rigBod = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInitialRotation = transform.eulerAngles;
        oldPlayerPosY = transform.position.y;
    }

    // Update is called once per frame
    private void Update() {
        GetKeyboardInput();
        ProcessKeyboardInput();
    }

    private void LateUpdate() {
        AnimateCharacter();
    }

    // get input for running
    private void GetKeyboardInput() {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetButtonDown("Jump");
    }

    // process input
    private void ProcessKeyboardInput() {
        if( jumpInput ) CheckPlayerJump();
        CheckMidAir();
    }
    private void CheckPlayerJump() {
        if( !isJumping && !isFalling ) {
            rigBod.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
            isFalling = false;
        }
    }
    private void CheckMidAir() {
        // check if jump is aborted by pressing S
        if( moveY > 0 ) {
            moveY = 0;
        } else if( moveY < 0 ) {
            rigBod.AddForce(new Vector2(0f, -jumpForce * .5f));
            if( isJumping ) {
                isFalling = true;
                isJumping = false;
            }
        }
        transform.position += new Vector3(moveX, 0f) * Time.deltaTime * moveSpeed;
        // check if player character is falling
        if( oldPlayerPosY > transform.position.y && initGroundTouch ) {
            isFalling = true;
            isJumping = false;
        }
        oldPlayerPosY = transform.position.y;
    }

    // animation for the movement
    private void AnimateCharacter() {
        // todo first check if jumping or falling, 
        // then check if walking
        // else idle
        // even when jumping or falling sprite needs to flip

        SetSpriteDirection();

        if (isJumping || isFalling) {
            SetJumpAnim();
        }else if( moveX != 0) {
            SetWalkAnim(true);
        } else {
            SetWalkAnim(false);
        }
    }
    private void SetSpriteDirection() {
        if( moveX < 0 ) {
            spriteRenderer.flipX = true;
            runningLeft = true;
            runningRight = false;
        } else if( moveX > 0 ) {
            spriteRenderer.flipX = false;
            runningLeft = false;
            runningRight = true;
        } else if( moveX == 0 ) {
            runningLeft = false;
            runningRight = false;
        }

        // player sprite should not rotate when hitting something
        transform.eulerAngles = playerInitialRotation;
    }
    private void SetJumpAnim() {
        if( !jumpAnimPlaying ) {
            animator.SetTrigger(JUMP_ANIMATION);
            jumpAnimPlaying = true;
        }
    }
    private void SetWalkAnim(bool input) {
        animator.SetBool(WALK_ANIMATION, input);
        jumpAnimPlaying = false;
    }

    // collision
    private void OnCollisionEnter2D(Collision2D collision) {
        if( collision.gameObject.CompareTag(GROUND_TAG) || collision.gameObject.CompareTag(PLATTFORM_TAG) ) {
            isJumping = false;
            isFalling = false;
            initGroundTouch = true;
        }
    }

    private void OnCollisionStay2D( Collision2D collision ) {
        if(collision.gameObject.CompareTag(GROUND_TAG) || collision.gameObject.CompareTag( PLATTFORM_TAG ) ) {
            isFalling = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if( !isJumping ) isFalling = true;
        // create falling animation 
        // replace with the setJumpAnim
    }
}
