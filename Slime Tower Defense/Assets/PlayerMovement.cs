using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody body;
 
    
    // Start is called before the first frame update
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;

        readyToJump = true;
    }

    // Update is called once per frame
    private void Update()
    {
        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        speedControl();

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
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Allows jumping
        if(grounded && readyToJump && Input.GetKey(jumpKey))
        {
            readyToJump = false;

            jump();

            // Calls resetJump with the cd as the delay, allowing for continious jumping if space is held
            Invoke(nameof(resetJump), jumpCooldown);
        }

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

    private void jump()
    {
        // Reset y-velocity
        body.velocity = new Vector3(body.velocity.x, 0f, body.velocity.z);

        // ForceMode.Impulse applies the force ONCE 
        body.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void resetJump()
    {
        // Resets readyToJump to true again
        readyToJump = true;
    }
}
