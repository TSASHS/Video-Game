using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class LevelMain : MonoBehaviour
{
    private bool pause = false;
    public bool onGround, onLadder;
    private float y2;
    public float mouseSensitivity, groundDistance, lightGroundDistance, jumpHeight, gravity, speed;
    public Vector3 fallVelocity;
    public LayerMask ground, ladder;
    public Camera mainCamera;
    public CharacterController controller;
    public Animator torchAnim;
    public GameObject torchLight, walkingAudio, pauseMenu, whitedDot, confirmObj;
    public Transform groundCheck;
    private Transform torchLightPos;
    public TMP_InputField inputField;
    GameObject[] objs;

    //Load Game functions in Awake Function
    void Awake()
    {
        objs = GameObject.FindGameObjectsWithTag("LoadSystem");
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
            
            torchAnim.gameObject.transform.rotation = new Quaternion(67.5f,0,0,0);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(objs.Length > 0){
            PlayerData data = objs[0].GetComponent<StorageClass>().data;
            torchAnim.SetBool("Torch", data.torchState);
            print(torchAnim.gameObject.transform.rotation.x);
        }
        torchLightPos = torchLight.transform;
        Time.timeScale = 1;   
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
            torchAnim.SetBool("Walking", true);
        }else{
            walkingAudio.SetActive(false);
            torchAnim.SetBool("Walking", false);
        }
        if(Input.GetKeyDown(KeyCode.T)){
            print("something");
            Torch();
        }
    }
    void Pause()
    {
    }
    void OnDrawGizmos ()
    {
        Gizmos.DrawSphere(groundCheck.position, groundDistance);
    }
    void Movement()
    {
        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, ground);
        onLadder= Physics.Raycast(groundCheck.position, controller.transform.forward ,groundDistance*4, ladder);
        if(!onGround){
            onGround = onLadder;
        }
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 movementVector;
        if(onLadder == true){
            movementVector = controller.transform.right * x + controller.transform.up * y;
        }else{
            movementVector = controller.transform.right * x + controller.transform.forward * y;   
        }

        fallVelocity.y -= gravity * Time.deltaTime;

        if(onGround && fallVelocity.y < 0){
            fallVelocity.y = 0f;
        }
        if(Input.GetButtonDown("Jump") && onGround){
            fallVelocity.y = Mathf.Sqrt(jumpHeight * -2f * -gravity);
        }
        controller.Move(fallVelocity * Time.deltaTime);
        
        if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)){
            controller.Move(movementVector * speed *  Time.deltaTime);
        }
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
        bool torch = !torchAnim.GetBool("Torch");
        print(torch);
        torchAnim.SetBool("Torch", !torch);
        print(torchAnim.GetBool("Torch"));
        torchAnim.SetBool("b", true);
    }
    public void SaveGame()
    {
        SaveSystem.SavePlayer(this, inputField.text, confirmObj);
        inputField.transform.parent.gameObject.SetActive(false);        
    }
    public void ConfirmSaveGame()
    {
        SaveSystem.SavePlayer(this, inputField.text);
        confirmObj.transform.parent.gameObject.SetActive(false);  
    }
    public void Restart ()
    {
        SceneManager.LoadScene("Loading2");
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
