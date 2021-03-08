using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMain : MonoBehaviour
{
    private bool pause, onGround;
    private float y2;
    public float mouseSensitivity, groundDistance;
    public LayerMask ground;
    public Camera mainCamera;
    public CharacterController character;
    public Transform groundCheck;

    //Load Game functions in Awake Function
    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
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
        if(Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.D)){
            Movement();
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
