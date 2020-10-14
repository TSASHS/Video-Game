using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementScript : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 1f;
    public float gravity = 7;
    public Vector3 fallVelocity;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 1.2f;
    public LayerMask ground;
    public bool onGround = true;

    // Update is called once per frame
    void Update()
    {
        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, ground);

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 movementVector = transform.right * x + transform.forward * y;

        fallVelocity.y -= gravity * Time.deltaTime;
        
        if(onGround && fallVelocity.y < 0){
            fallVelocity.y = -0.6f;
        }
        if(Input.GetButtonDown("Jump") && onGround){
            fallVelocity.y = Mathf.Sqrt(jumpHeight * -2f * -gravity);
        }
        controller.Move(fallVelocity * Time.deltaTime);

        controller.Move(movementVector * speed *  Time.deltaTime);
    }
}
