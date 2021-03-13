using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public CharacterController controller;
    public Camera mainCamera;
    public float mouseSensitivity, groundDistance, gravity, jumpHeight, speed;
    private float y2 = 0;
    private bool onGround, torch, pause, timeDone;
    public bool challengeCompleted;
    public LayerMask ground, interactable;
    public Transform groundCheck;
    public Vector3 movementVector, fallVelocity;
    public Animator torchAnim;
    public List<Animator> animators = new List<Animator>();
    public List<bool> onOffList = new List<bool>();
    public List<bool> leverList = new List<bool>();
    private Transform torchLightPos;
    private float tick = 0;
    private List<float> timeList = new List<float>();
    public Light torchLight;
    public GameObject eSprite, pauseMenu, cursor, walkingAudioObject;
    public SceneChanger _SceneChanger;
    public TMP_InputField inputField;
    public GameObject confirmObj;
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("LoadSystem");
        if(objs.Length > 0){
            PlayerData data = objs[0].GetComponent<StorageClass>().data;
            Vector3 positon = new Vector3(data.position[0], data.position[1], data.position[2]);
            Quaternion rotation = new Quaternion(data.rotation[1], data.rotation[2], data.rotation[3], data.rotation[0]);
            Quaternion cameraRotation = new Quaternion(data.cameraRotation[1], data.cameraRotation[2], data.cameraRotation[3], data.cameraRotation[0]);
            controller.enabled = false;
            controller.transform.position = positon;
            controller.enabled = true;
            controller.transform.rotation = rotation;
            controller.transform.GetChild(2).rotation = cameraRotation;
            torchAnim.SetBool("Torch", data.torchState);
            if(data.level2Floor == true){
                animators[20].SetBool("ChallengeCompleted", true);
            }
            for(int i = 0; i < 5; i++){
                bool b = data.levelTwoPositions[i];
                animators[i].SetBool("LeverDown", b);
                animators[i+5].SetBool("LeverDown", b);
                animators[i+10].SetBool("LeverDown", b);
                onOffList[i] = b;
                leverList[i] = data.leverPositions[i];
                animators[i+15].SetBool("LeverDown", data.leverPositions[i]);
            }
            Destroy(objs[0]);
        }
    }
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
            challengeCompleted = true;
            print("Challenge Complete");
            animators[20].SetBool("ChallengeCompleted", true);
        }
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            torchAnim.SetBool("Walking", true);
            walkingAudioObject.SetActive(true);
        }else {
            torchAnim.SetBool("Walking", false);
            walkingAudioObject.SetActive(false);  
        }
        LightFLicker();
        if(Input.GetKeyDown(KeyCode.T)){
            Torch();
        }
    }

    void Pause()
    {
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
                    bool leverState = animators[id + 15].GetBool("LeverDown");
                    animators[id+15].SetBool("LeverDown", !leverState);
                    animators[id].SetBool("LeverDown", !currentState);
                    animators[id + 5].SetBool("LeverDown", !currentState);
                    animators[id+10].SetBool("LeverDown", !currentState);
                    onOffList[id] = !onOffList[id];
                    leverList[id] = !leverList[id];
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
        torch = !torchAnim.GetBool("Torch");
        print(torch);
        torchAnim.SetBool("Torch", torch);
        torchAnim.SetBool("b", true);
        print(torchAnim.GetBool("Torch"));
    }
    public void LeaveRoom()
    {
       _SceneChanger.sceneChange("Loading2");
    }
    public void SavePlayer ()
    {
        SaveSystem.SavePlayer(this, inputField.text, confirmObj);
        inputField.transform.parent.gameObject.SetActive(false);
    }
    public void ConfirmSavePlayer ()
    {
        SaveSystem.SavePlayer(this, inputField.text);
        confirmObj.transform.parent.gameObject.SetActive(false);  
    }
    public void Restart ()
    {
        SceneManager.LoadScene("Loading - Copy");
    }
    public void musicVolume(float volume)
    {
        GameObject [] musicObjs = GameObject.FindGameObjectsWithTag("music");
        musicObjs[0].GetComponent<AudioSource>().volume = volume;
    }
    public void SFXVolume(float volume)
    {
        GameObject [] sfxObjs = GameObject.FindGameObjectsWithTag("SFX");
        foreach (GameObject o in sfxObjs){
            o.GetComponent<AudioSource>().volume = volume;
        }
    }
}