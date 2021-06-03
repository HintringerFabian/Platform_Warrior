using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private readonly float moveSpeed = 10f;
    private readonly float jumpForce = 12f;

    private float oldPlayerPosY;
    private float moveX;
    private float moveY;

    private bool jumpInput;
    private bool jumpAnimPlaying;
    private bool initGroundTouch = false;
    private bool runningLeft;
    private bool runningRight;
    private bool isJumping;
    private bool isFalling = false;

    public bool IsJumping { get => isJumping; }
    public bool IsFalling { get => isFalling; }
    public bool RunningRight { get => runningRight; }
    public bool RunningLeft { get => runningLeft; }

    private Vector3 playerInitialRotation;

    private Rigidbody2D rigBod;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private const string WALK_ANIMATION = "Walk",
        JUMP_ANIMATION = "Jump",
        PLATTFORM_TAG = "Plattform",
        GROUND_TAG = "Ground";

    private void Start() {

        rigBod = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerInitialRotation = transform.eulerAngles;
        oldPlayerPosY = transform.position.y;
    }

    private void Update() {

        // get keyboard input
        moveX = Input.GetAxisRaw( "Horizontal" );
        moveY = Input.GetAxisRaw( "Vertical" );
        jumpInput = Input.GetButtonDown( "Jump" );

        // process keyboard input
        if ( jumpInput ) {

            if ( !isJumping && !isFalling ) {
                rigBod.AddForce( new Vector2( 0f , jumpForce ) , ForceMode2D.Impulse );
                isJumping = true;
                isFalling = false;
            }
        }

        CheckMidAirMovement();
    }

    private void LateUpdate() {

        // Animate character
        SetSpriteDirection();

        if ( isJumping || isFalling ) {
            SetJumpAnim();

        } else if ( moveX != 0 ) {
            SetWalkAnim( true );

        } else {
            SetWalkAnim( false );
        }
    }

    private void CheckMidAirMovement() {

        // check if jump is aborted (by pressing S on keyboard)
        if ( moveY < 0 ) {
            rigBod.AddForce( new Vector2( 0f , -jumpForce * .5f ) );

            if ( isJumping ) {
                isFalling = true;
                isJumping = false;
            }
        } else moveY = 0;

        transform.position += new Vector3( moveX , 0f ) * Time.deltaTime * moveSpeed;

        // check if player character is falling
        if ( oldPlayerPosY > transform.position.y && initGroundTouch ) {
            isFalling = true;
            isJumping = false;
        }

        oldPlayerPosY = transform.position.y;
    }

    private void SetSpriteDirection() {

        if ( moveX < 0 ) {
            spriteRenderer.flipX = true;
            runningLeft = true;
            runningRight = false;

        } else if ( moveX > 0 ) {
            spriteRenderer.flipX = false;
            runningLeft = false;
            runningRight = true;

        } else if ( moveX == 0 ) {
            runningLeft = false;
            runningRight = false;
        }

        // player sprite should not rotate when hitting something
        transform.eulerAngles = playerInitialRotation;
    }
    private void SetJumpAnim() {

        if ( !jumpAnimPlaying ) {
            animator.SetTrigger( JUMP_ANIMATION );
            jumpAnimPlaying = true;
        }
    }
    private void SetWalkAnim( bool input ) {

        animator.SetBool( WALK_ANIMATION , input );
        jumpAnimPlaying = false;
    }

    // collision
    private void OnCollisionEnter2D( Collision2D collision ) {

        if ( collision.gameObject.CompareTag( GROUND_TAG ) || collision.gameObject.CompareTag( PLATTFORM_TAG ) ) {
            isJumping = false;
            isFalling = false;
            initGroundTouch = true;
        }
    }

    private void OnCollisionStay2D( Collision2D collision ) {

        if ( collision.gameObject.CompareTag( GROUND_TAG ) || collision.gameObject.CompareTag( PLATTFORM_TAG ) ) {
            isFalling = false;
        }
    }

    private void OnTriggerExit2D( Collider2D collision ) {

        if ( !isJumping ) isFalling = true;
        // create falling animation 
        // replace with the setJumpAnim
    }
}
