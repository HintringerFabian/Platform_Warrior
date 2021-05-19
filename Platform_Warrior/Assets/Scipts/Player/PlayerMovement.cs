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
        getKeyboardInput();
        processKeyboardInput();
    }

    private void LateUpdate() {
        animateCharacter();
    }

    // get input for running
    private void getKeyboardInput() {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetButtonDown("Jump");
    }

    // process input
    private void processKeyboardInput() {
        if( jumpInput ) checkPlayerJump();
        checkMidAir();
    }
    private void checkPlayerJump() {
        if( !isJumping && !isFalling ) {
            rigBod.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
            isFalling = false;
        }
    }
    private void checkMidAir() {
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
    private void animateCharacter() {
        // todo first check if jumping or falling, 
        // then check if walking
        // else idle
        // even when jumping or falling sprite needs to flip

        setSpriteDirection();

        if (isJumping || isFalling) {
            setJumpAnim();
        }else if( moveX != 0) {
            setWalkAnim(true);
        } else {
            setWalkAnim(false);
        }
    }
    private void setSpriteDirection() {
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
    private void setJumpAnim() {
        if( !jumpAnimPlaying ) {
            animator.SetTrigger(JUMP_ANIMATION);
            jumpAnimPlaying = true;
        }
    }
    private void setWalkAnim(bool input) {
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

    private void OnTriggerExit2D(Collider2D collision) {
        if( !isJumping ) isFalling = true;
        // create falling animation 
        // replace with the setJumpAnim
    }
}
