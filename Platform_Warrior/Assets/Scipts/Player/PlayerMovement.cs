using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 10f;
    private float jumpForce = 10f;
    private float maxVelocity = 20f;
    private float moveX;

    private Rigidbody2D rigBod;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private string WALK_ANIMATION = "Walk";
    private string JUMP_ANIMATION = "Jump";
    private string IDLE_ANIMATION = "Idle";

    private void Awake() {
        rigBod = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerKeyboardInput();
    }

    void playerKeyboardInput() {
        moveX = Input.GetAxisRaw("Horizontal");
        Debug.Log(moveX);
        transform.position += new Vector3(moveX, 0f, 0f) * Time.deltaTime * moveSpeed;
    }
}
