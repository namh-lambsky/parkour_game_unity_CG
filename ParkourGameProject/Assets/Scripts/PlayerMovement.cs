using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float movementSpeed;
    public float walkSpeed;
    public float sprintSpeed;


    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump=true;

    [Header("Keybinds")]
    public KeyCode jumpKey= KeyCode.Space;
    public KeyCode sprintKey= KeyCode.LeftShift;


    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;



    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    public enum MovementState   
    {
        walking,sprinting,air
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f+0.2f,whatIsGround);
        MyInput();
        SpeedControl();
        StateHandler();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else 
        { 
            rb.drag = 0; 
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    

    private void StateHandler()
    {
        if (grounded && Input.GetKey(KeyCode.LeftShift)) 
        {
            state = MovementState.sprinting;
            movementSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            movementSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

    }

    private void MovePlayer()
    {
        moveDirection=orientation.forward*verticalInput+orientation.right*horizontalInput;


        if (grounded) 
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);

        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f*airMultiplier, ForceMode.Force);

        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel=new Vector3(rb.velocity.x,0f,rb.velocity.z);

        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            rb.velocity=new Vector3(limitedVel.x,rb.velocity.y,limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up*jumpForce,ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }


}
