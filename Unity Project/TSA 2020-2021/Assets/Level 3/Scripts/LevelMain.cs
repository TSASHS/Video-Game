using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMain : MonoBehaviour
{
    private bool pause;
    public bool onGround;
    private float y2;
    public float mouseSensitivity, groundDistance, lightGroundDistance, jumpHeight, gravity, speed;
    public Vector3 fallVelocity;
    public LayerMask ground;
    public Camera mainCamera;
    public CharacterController controller;
    public GameObject torchLight, walkingAudio;
    public Transform groundCheck;
    private Transform torchLightPos;

    //Load Game functions in Awake Function
    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        torchLightPos = torchLight.transform;
        Time.timeScale = 1;   
        Cursor.lockState = CursorLockMode.Locked;
        pause = false;        
    }

    // Update is called once per frame
    void Update()
    {
        Look();
        if(Input.GetKeyDown(KeyCode.Escape)){
            Pause();
        }
        Movement();
        if((Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)) && onGround){
            walkingAudio.SetActive(true);
        }else{
            walkingAudio.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.T)){
            Torch();
        }
    }
    void Pause()
    {

    }
    void Movement()
    {
        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, ground);

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 movementVector = controller.transform.right * x + controller.transform.forward * y;
        
        print(movementVector.x);
        print(movementVector.z);

        fallVelocity.y -= gravity * Time.deltaTime;

        if(onGround && fallVelocity.y < 0){
            fallVelocity.y = 0f;
        }
        if(Input.GetButtonDown("Jump") && onGround){
            fallVelocity.y = Mathf.Sqrt(jumpHeight * -2f * -gravity);
        }
        controller.Move(fallVelocity * Time.deltaTime);

        controller.Move(movementVector * speed *  Time.deltaTime);
    }
    void Look()
    {
        float x = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        y2 -= y;
        y2 = Mathf.Clamp(y2, -90f, 90f);

        mainCamera.gameObject.transform.localRotation = Quaternion.Euler(y2, 0f, 0f);
        mainCamera.gameObject.transform.parent.Rotate(Vector3.up * x);   
    }
    void Torch()
    {
        
    }
    public void SaveGame()
    {

    }
    public void ConfirmSaveGame()
    {

    }
}
public class collisionClass : MonoBehaviour 
{
    void OnTriggerEnter()
    {
        //EndGame
    }
}
