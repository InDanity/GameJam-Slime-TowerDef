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
    public float airMultiplier;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody body;

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        air
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
    }

    // Update is called once per frame
    private void Update()
    {
        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        myInput();
        speedControl();
        stateHandler();

        // Handle drag
        if (grounded)
        {
            body.drag = groundDrag;
        }
        else
        {
            body.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    // Gets player input
    private void myInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

    }

    private void movePlayer()
    {
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // On ground
        if (grounded)
        {
            body.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);
        }

        // In air, multiply by airMultiplier
        else if(!grounded)
        {
            body.AddForce(moveDirection.normalized * movementSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        

    }

    // Without this, speed can go over set movement speed
    private void speedControl()
    {
        Vector3 flatVel = new Vector3(body.velocity.x, 0f, body.velocity.z);

        if(flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            body.velocity = new Vector3(limitedVel.x, body.velocity.y, limitedVel.z);
        }
    }

    private void stateHandler()
    {
        // Sprinting
        if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            movementSpeed = sprintSpeed;
        }

        // Walking
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


}
