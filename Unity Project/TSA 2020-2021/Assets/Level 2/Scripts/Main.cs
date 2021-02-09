using UnityEngine;

public class Main : MonoBehaviour
{
    public CharacterController controller;
    public Camera mainCamera;
    public float mouseSensitivity, groundDistance, gravity, jumpHeight, speed;
    private float y2 = 0;
    private bool onGround, torch, challengeCompleted;
    public LayerMask ground, interactable;
    public Transform groundCheck;
    public Vector3 movementVector, fallVelocity;
    private Transform torchLightPos;
    private float tick = 0;
    public Light torchLight;
    public GameObject eSprite;
    public SceneChanger _SceneChanger;

    void Start ()
    {   
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        torchLightPos = torchLight.transform;
    }
    void Update ()
    {
        tick++;
        LookAround();
        Movement();
    }
    void LookAround()
    {
        float x = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        y2 -= y;
        y2 = Mathf.Clamp(y2, -90f, 90f);

        mainCamera.gameObject.transform.localRotation = Quaternion.Euler(y2, 0f, 0f);
        mainCamera.gameObject.transform.parent.Rotate(Vector3.up * x);
    }
    void Movement ()
    {
        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, ground);

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 movementVector = controller.transform.right * x + controller.transform.forward * y;

        if(movementVector.y > 0){
            if(Physics.Raycast(torchLightPos.position, -torchLightPos.up, 0.01f)){
                movementVector.y = 0;
            }
        }else{
            if(Physics.Raycast(torchLightPos.position, torchLightPos.up, 0.01f)){
                movementVector.y = 0;
            }
        }

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
    void LightFLicker ()
    {
        if(tick == 5){
            tick = 0;
            float intensity = torchLight.intensity + Random.Range(-0.003f,0.003f);
            intensity = Mathf.Clamp(intensity, 0.03f, 0.05f);
            torchLight.intensity = intensity;
        }
    }
    void ESpriteFunc ()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 2f,interactable)){
            if(hit.transform.gameObject.GetComponent<InteractableObj>().interactable == true && challengeCompleted == false){
                eSprite.SetActive(true);
                if(Input.GetKeyDown(KeyCode.E)){
                    //puzzleCode
                }
            }
        }else{
            eSprite.SetActive(false);
        }
    }
    /*void Torch()
    {
        torch = !torch;
        print(torch);
        animator1.SetBool("Torch", torch);
        print(animator1.GetBool("Torch"));
    }*/
    void LeaveRoom()
    {
       _SceneChanger.sceneChange("Level 3");
    }
}