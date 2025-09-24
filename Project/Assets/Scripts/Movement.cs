using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController player;

    public float speed = 12f; // walking speed
    public float sprintSpeed = 18f; // sprinting speed
    public float gravity = -9.81f;

    public Transform groundCheck; // checks for player states
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool isSprinting;

    public KeyCode sprintKey = KeyCode.LeftShift;

    public void setRunSpeed(float speed)
    {
        sprintSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // small neg to keep grounded
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        isSprinting = Input.GetKey(sprintKey);
        float currentSpeed = isSprinting ? sprintSpeed : speed;

        Vector3 move = transform.right * x + transform.forward * z;
        player.Move(move.normalized * currentSpeed * Time.deltaTime);

        // gravity
        velocity.y += gravity * Time.deltaTime;
        player.Move(velocity * Time.deltaTime);
    }
}