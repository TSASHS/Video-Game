using UnityEngine;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
    public CharacterController controller;
    public Camera mainCamera;
    public float mouseSensitivity, groundDistance, gravity, jumpHeight, speed;
    private float y2 = 0;
    private bool onGround, torch, challengeCompleted, pause, timeDone;
    public LayerMask ground, interactable;
    public Transform groundCheck;
    public Vector3 movementVector, fallVelocity;
    public Animator torchAnim;
    public List<Animator> animators = new List<Animator>();
    public List<bool> onOffList = new List<bool>();
    private Transform torchLightPos;
    private float tick = 0;
    private List<float> timeList = new List<float>();
    public Light torchLight;
    public GameObject eSprite, pauseMenu, cursor, walkingAudioObject;
    public SceneChanger _SceneChanger;

    void Start ()
    {   
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        torchLightPos = torchLight.transform;
        challengeCompleted = false;
        pause = false;
    }
    void Update ()
    {
        tick++;
        LookAround();
        Movement();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        ESpriteFunc();
        int trueCount = 0;
        foreach(bool b in onOffList){
            if(b == true)
            {
                trueCount ++;
            }
        }
        if (trueCount == 5){
            print("Challenge Complete");
        }
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            walkingAudioObject.SetActive(true);
        }else {
            walkingAudioObject.SetActive(false);  
        }
        LightFLicker();
        if(Input.GetKeyDown(KeyCode.T)){
            Torch();
        }
    }

    void Pause()
    {
        pause = !pause;
        pauseMenu.SetActive(pause);
        if(pause==true){
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            cursor.SetActive(false);
        }else{
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            cursor.SetActive(true);
        }
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
            float intensity = torchLight.intensity + Random.Range(-0.01f,0.01f);
            intensity = Mathf.Clamp(intensity, 0.25f, 0.75f);
            torchLight.intensity = intensity;
        }
    }
    void ESpriteFunc ()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 20f,interactable)){
            if(hit.transform.gameObject.GetComponent<InteractableObj>().interactable == true && challengeCompleted == false){
                GameObject selectedObj = hit.transform.gameObject;
                InteractableObj selectedObjInteractable = selectedObj.GetComponent<InteractableObj>();
                int id = selectedObjInteractable.id;
                eSprite.SetActive(true);
                if(Input.GetKeyDown(KeyCode.E)){
                    print("z");
                    bool currentState = animators[id].GetBool("LeverDown");
                    animators[id].SetBool("LeverDown", !currentState);
                    animators[id + 5].SetBool("LeverDown", !currentState);
                    animators[id+10].SetBool("LeverDown", !currentState);
                    animators[id+15].SetBool("LeverDown", !currentState);
                    onOffList[id] = !onOffList[id];
                    if(id != 0){
                        currentState = animators[id - 1].GetBool("LeverDown");
                        animators[id - 1].SetBool("LeverDown", !currentState);
                        animators[id + 4].SetBool("LeverDown", !currentState);
                        animators[id + 9].SetBool("LeverDown", !currentState);
                        onOffList[id - 1] = !onOffList[id - 1];   
                    }else{
                        currentState = animators[4].GetBool("LeverDown");
                        animators[4].SetBool("LeverDown", !currentState);
                        animators[9].SetBool("LeverDown", !currentState);
                        animators[14].SetBool("LeverDown", !currentState);
                        onOffList[4] = !onOffList[4];                        
                    }
                    if(id != 4){
                        currentState = animators[id + 1].GetBool("LeverDown");
                        animators[id + 1].SetBool("LeverDown", !currentState);
                        onOffList[id + 1] = !onOffList[id + 1];
                        animators[id + 6].SetBool("LeverDown", !currentState);
                        animators[id + 11].SetBool("LeverDown", !currentState);
                    }else{
                        currentState = animators[0].GetBool("LeverDown");
                        animators[0].SetBool("LeverDown", !currentState);
                        animators[5].SetBool("LeverDown", !currentState);
                        animators[10].SetBool("LeverDown", !currentState);
                        onOffList[0] = !onOffList[0];                        
                    }
                }
            }
        }else{
            eSprite.SetActive(false);
        }
    }
    void Torch()
    {
        torch = !torch;
        print(torch);
        torchAnim.SetBool("Torch", torch);
        print(torchAnim.GetBool("Torch"));
    }
    void LeaveRoom()
    {
       _SceneChanger.sceneChange("Level 3");
    }
}