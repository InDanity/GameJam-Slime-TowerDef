using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    Rigidbody rb;
    public float jumpHeight = 10;
    public bool grounded;
    public int maxJumpCount = 2;
    public int jumpsRemaining = 0;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grounded && (Input.GetKeyDown(KeyCode.Space)) && (jumpsRemaining > 0))
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            jumpsRemaining -= 1;
        }

        // If already in the air, negate current y-velocity
        else if(!grounded && (Input.GetKeyDown(KeyCode.Space)) && (jumpsRemaining > 0))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            jumpsRemaining -= 1;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            grounded = true;
            jumpsRemaining = maxJumpCount;
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
            grounded = false;
    }
}
