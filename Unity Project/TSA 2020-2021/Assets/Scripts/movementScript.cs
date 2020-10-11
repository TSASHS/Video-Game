using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementScript : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 1f;
    public float gravity = 7;
    public Vector3 fallVelocity;

    public LayerMask ground;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 movementVector = transform.right * x + transform.forward * y;

        fallVelocity.y -= gravity * Time.deltaTime;

        controller.Move(fallVelocity * Time.deltaTime);

        controller.Move(movementVector * speed *  Time.deltaTime);
    }
}
