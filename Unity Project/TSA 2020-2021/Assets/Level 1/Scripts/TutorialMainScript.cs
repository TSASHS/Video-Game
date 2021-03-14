using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class TutorialMainScript : MonoBehaviour
{
    float tick = 0;
    public GameObject tutorialSprite;
    public GameObject player, keys, circleOnGround, pauseMenu;
    GameObject playerCamera;
    TextMeshProUGUI tutorialText;
    public TextMeshProUGUI wText, aText, sText, dText;
    public GameObject textObjHolder;
    public LayerMask walkToMask;
    public int tutorialStage = 0;
    public bool secondCountBool, wClicked, aClicked, dClicked, sClicked = false;
    float secondCount = 0;
    public bool interactable0;
    public GameObject eText;
    public LayerMask interactableLayer, cubes, endTunnel;
    public Camera mainCamera, interactCamera0;
    public Transform torchLightPos;
    public bool challengeCompleted = false;
    public Animator animator, animator2, animator1;
    public List<GameObject> cubeList = new List<GameObject>();
    public List<Image> circleList = new List<Image>();
    public bool challenge = false;
    public GameObject eObj, whitePoint;
    public float mouseSensitivity = 120f;
    float y2 = 0;
    float start;
    public CharacterController controller;
    public float speed = 1f;
    public float gravity = 7;
    public Vector3 fallVelocity;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.00000000000000001f;
    public LayerMask ground;
    public LayerMask ground2;
    public bool onGround = true;
    public Color32 circleColor = new Color32(41, 108, 164, 255);
    private Color circleColor1;
    private GameObject blueCube;
    private bool moving;
    private Dictionary<int, int> pictureRandomDict = new Dictionary<int, int>();
    public List<Texture2D> pictureTextureList = new List<Texture2D>();
    public Light torchLight;
    public bool enteredRoom = false;
    public SceneChanger _SceneChanger;
    float y;
    private bool torch = true;
    public TMP_InputField inputField;
    public GameObject confirmObj, walking, settingsMenu;
    // Start is called before the first frame update
    // Update is called once per frame
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

            tutorialStage = data.tutorialStage;

            foreach(string s in data.buttonsClicked){
                if(s == "w"){
                    wClicked = true;
                    wText.color = Color.green;
                }else if (s == "a"){
                    aClicked = true;
                    aText.color = Color.green;
                }else if (s == "s"){
                    sText.color = Color.green;
                    sClicked = true;
                }else if (s == "d"){
                    dText.color = Color.green;
                    dClicked = true;
                }
            }
            if(data.tutorialDoor2 == true){
                animator.SetBool("ChallengeCompleted", true);
            }
            if(data.tutorialDoor == true){
                animator2.gameObject.transform.position = new Vector3(-7.872f,0.519f,-0.1583f);
            }

            for(int i = 0; i < cubeList.Count; i++){
                Puzzle1Node node = new Puzzle1Node();
                Puzzle1Cube cube = cubeList[i].GetComponent<Puzzle1Cube>();
                foreach(GameObject g in cubeList){
                    if(g.GetComponent<Puzzle1Cube>().node.id == data.firstLevelBoxPositionsid[i]){
                        node = g.GetComponent<Puzzle1Cube>().node;
                    }
                }
                cube.node = node;
                cubeList[i].transform.position = new Vector3(data.firstLevelBoxPositionsx[i], data.firstLevelBoxPositionsy[i], cubeList[i].transform.position.z);
            }
            animator1.SetBool("Torch", data.torchState);
            Destroy(objs[0]);
        }
    }
    void Start()
    {
        playerCamera = player.transform.GetChild(2).gameObject;
        tutorialText = tutorialSprite.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tutorialSprite.SetActive(true);
        y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        controller.transform.GetChild(0).gameObject.SetActive(false);
        pictureRandomDict.Add(0,5);
        pictureRandomDict.Add(1,0);
        pictureRandomDict.Add(2,7);
        pictureRandomDict.Add(3,1);
        pictureRandomDict.Add(4,8);
        pictureRandomDict.Add(5,2);
        pictureRandomDict.Add(6,4);
        pictureRandomDict.Add(7,3);
        pictureRandomDict.Add(8,6);
        Cursor.lockState = CursorLockMode.Locked;
        start = -Input.GetAxis("Mouse Y");
        circleColor1 = circleColor;
        for(int i = 0; i < cubeList.Count; i++){
            cubeList[i].GetComponent<Renderer>().material.shader = Shader.Find("Shader Graphs/Outline");
            cubeList[i].GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.black);
            cubeList[i].GetComponent<Renderer>().material.SetTexture("_AO", pictureTextureList[pictureRandomDict[i]]);
            cubeList[i].GetComponent<Renderer>().material.SetTexture("_Diff", pictureTextureList[pictureRandomDict[i]+9]);
            cubeList[i].GetComponent<Renderer>().material.SetTexture("_Nor", pictureTextureList[pictureRandomDict[i]+18]);
            cubeList[i].GetComponent<Renderer>().material.SetTexture("_Spec", pictureTextureList[pictureRandomDict[i]+27]);
            cubeList[i].GetComponent<Puzzle1Cube>().node.pos = cubeList[i].transform.position;
        }
        torchLightPos = torchLight.transform;
    }
    void Update()
    {
        tick += 1;
        if(pauseMenu.activeSelf != true){
            Challenge1();
            eTextFunc();
        }
        if(challenge != true){
            if(tutorialStage > 0){
                LookAround();
            }
            if(tutorialStage > 1){
                Movement();
            }
        }
        Tutorial();
        LightFLicker();
        if(enteredRoom == false && Physics.CheckSphere(groundCheck.position, groundDistance, ground2) == true){
            print("enteredRoom");
            enteredRoom = true;
            animator2.SetBool("InRoom", true);
        }
        if(Physics.CheckSphere(groundCheck.position, groundDistance, endTunnel) == true){
            LeaveRoom();
        }
        if(Input.GetKeyDown(KeyCode.T) && tutorialStage > 2){
            Torch();
        }
        if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)){
            animator1.SetBool("Walking", true);
            walking.SetActive(true);
        }else{
            animator1.SetBool("Walking", false);
            walking.SetActive(false);
        }
    }
    void Tutorial ()
    {
        if(tutorialStage == 0){
            tutorialText.text = "Press the escape key to pause the game";
            if(Input.GetKeyDown(KeyCode.Escape)){
                secondCountBool = true;
                tutorialText.color = Color.green;
            }
        }

        if(tutorialStage == 1){
            tutorialText.text = "Move the cursor to look around";
            if(Mathf.Abs(player.transform.rotation.y) > 5/360|| Mathf.Abs(playerCamera.transform.localRotation.x) > 5/360){
                secondCountBool = true;
                tutorialText.color = Color.green;
            }
        }
        if(tutorialStage == 2){
            textObjHolder.SetActive(true);
            tutorialText.text = "Use the          keys to move";
            if(Input.GetKeyDown(KeyCode.W)){
                wClicked = true;
                wText.color = Color.green;
            }
            if(Input.GetKeyDown(KeyCode.A)){
                aClicked = true;  
                aText.color = Color.green;              
            }
            if(Input.GetKeyDown(KeyCode.D)){
                dClicked = true;
                dText.color = Color.green;
            }
            if(Input.GetKeyDown(KeyCode.S)){
                sClicked = true;
                sText.color = Color.green;
            }
            if(wClicked == true && aClicked == true&& dClicked == true && sClicked == true){
                tutorialText.color = Color.green;
                secondCountBool = true;
            }
        }else{
            textObjHolder.SetActive(false);
        }
        if(tutorialStage == 3){
            textObjHolder.SetActive(false);
            tutorialText.text = "Press t to light or extinguish your torch";
            if(Input.GetKeyDown(KeyCode.T)){
                tutorialText.color = Color.green;
                secondCountBool = true;
            }
        }
        if(tutorialStage == 4){
            circleOnGround.SetActive(true);
            tutorialText.text = "Walk to the marked location";
            if(Physics.CheckSphere(groundCheck.position, groundDistance, walkToMask)){
                tutorialText.color = Color.green;
                secondCountBool = true;
            }
        }else{
            circleOnGround.SetActive(false);
        }
        if(tutorialStage == 5){
            tutorialText.text = "Click E to interact with the picture";
            if(challenge == true){
                tutorialText.color = Color.green;
                secondCountBool = true;
            }
        }
        if(tutorialStage == 6){
            tutorialText.text = "Rearrange the blocks to form the picture";
        }
        if(tutorialStage == 7){
            tutorialText.text = "Walk through the tunnel to the next room";
        }
        if(secondCountBool == true){
            secondCount += Time.deltaTime;
            if(secondCount > 1){
                secondCountBool = false;
                tutorialText.color = Color.white;
                tutorialStage += 1;
                secondCount = 0;
            }
        }
    }
    void LightFLicker()
    {
        if(tick == 5){
            tick = 0;
            float intensity = torchLight.intensity + Random.Range(-0.003f,0.003f);
            intensity = Mathf.Clamp(intensity, 0.03f, 0.05f);
            torchLight.intensity = intensity;
        }
    }
    void Challenge1()
    {
        if(moving == true)
        {
            cubeList[1].GetComponent<AudioSource>().Play();
            Vector3 oldPos = blueCube.transform.position;
            Vector3 newPos = blueCube.GetComponent<Puzzle1Cube>().node.pos;
            if(oldPos != newPos){
                if(oldPos.x > newPos.x){
                    blueCube.transform.position -= Vector3.right * 1.75f * Time.deltaTime;
                    blueCube.transform.position = new Vector3(Mathf.Clamp(blueCube.transform.position.x, newPos.x, oldPos.x), blueCube.transform.position.y, oldPos.z);
                }else if (oldPos.x < newPos.x){
                    blueCube.transform.position += Vector3.right * 1.75f * Time.deltaTime;
                    blueCube.transform.position = new Vector3(Mathf.Clamp(blueCube.transform.position.x, oldPos.x, newPos.x), blueCube.transform.position.y, oldPos.z);
                }
                if(oldPos.y > newPos.y){
                    blueCube.transform.position -= Vector3.up * 1.75f * Time.deltaTime;
                    blueCube.transform.position = new Vector3(blueCube.transform.position.x, Mathf.Clamp(blueCube.transform.position.y, newPos.y, oldPos.y), oldPos.z);                        
                }else if (oldPos.y < newPos.y){
                    blueCube.transform.position += Vector3.up * 1.75f * Time.deltaTime;
                    blueCube.transform.position = new Vector3(blueCube.transform.position.x, Mathf.Clamp(blueCube.transform.position.y, oldPos.y, newPos.y), oldPos.z);
                }
            }else{
                moving = false;
                blueCube = null;
                
            }
        }else{
            cubeList[1].GetComponent<AudioSource>().Stop();
        }
        challenge = interactable0;
        if(challenge == true){
            eObj.SetActive(false);
            if(Input.GetKeyDown(KeyCode.E)){
                whitePoint.SetActive(true);
                interactCamera0.enabled = false;
                interactable0 = false;
                mainCamera.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if(Input.GetMouseButtonDown(0) && moving == false){
                RaycastHit hit;
                Ray ray = interactCamera0.ScreenPointToRay(Input.mousePosition);
                cubeList[8].GetComponent<Puzzle1Cube>().node.image.color = Color.clear;                
                if(blueCube != null){
                    blueCube.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.black);
                }
                if(Physics.Raycast(ray, out hit, 10000f, cubes)){
                    if(cubeList.Contains(hit.transform.gameObject)){
                        GameObject selectedCube = hit.transform.gameObject;
                        selectedCube.GetComponent<Renderer>().material.SetColor("_OutlineColor", circleColor1);
                        if(blueCube == selectedCube){
                            blueCube = null;
                            selectedCube.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.black);
                            return;
                        }
                        if(blueCube != null && selectedCube == cubeList[8] && selectedCube.GetComponent<Puzzle1Cube>().node.neighbours.Contains(blueCube.GetComponent<Puzzle1Cube>().node)){
                            Puzzle1Node oldNode = blueCube.GetComponent<Puzzle1Cube>().node;
                            blueCube.GetComponent<Puzzle1Cube>().node = cubeList[8].GetComponent<Puzzle1Cube>().node;
                            cubeList[8].GetComponent<Puzzle1Cube>().node = oldNode;
                            moving = true;
                            selectedCube.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.black);                           
                            cubeList[8].GetComponent<Puzzle1Cube>().node.image.color = Color.clear; 
                            cubeList[8].transform.position = oldNode.pos;                           
                            return;
                        }
                        blueCube = selectedCube;
                        if(selectedCube.GetComponent<Puzzle1Cube>().node.neighbours.Contains(cubeList[8].GetComponent<Puzzle1Cube>().node)){
                            cubeList[8].GetComponent<Puzzle1Cube>().node.image.color = circleColor;
                        }              
                    }else{
                        blueCube = null;
                    }
                }
            }

            int correctCubes = 0;
            for(int i = 0; i < cubeList.Count; i++){
                int correctID;
                if(pictureRandomDict[i] < 2){
                    correctID = pictureRandomDict[i] + 3;
                }else if (pictureRandomDict[i] > 5){
                    correctID = pictureRandomDict[i];
                }else {
                    correctID = pictureRandomDict[i] - 3;
                }
                if(cubeList[i].GetComponent<Puzzle1Cube>().node.id == correctID){
                    correctCubes += 1;
                }
            }
            if(correctCubes == 8){
                challengeCompleted = true;
                whitePoint.SetActive(true);
                interactCamera0.enabled = false;
                interactable0 = false;
                mainCamera.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                                Cursor.visible = false;
                animator.SetBool("ChallengeCompleted", true);
                tutorialText.color = Color.green;
                secondCountBool = true;
            }
        }
    }
    void eTextFunc (){
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 2f,interactableLayer)){
            if(hit.transform.gameObject.GetComponent<InteractableObj>().interactable == true && challenge == false && challengeCompleted == false){
                eText.SetActive(true);
                if(Input.GetKeyDown(KeyCode.E)){
                    if(hit.transform.gameObject.GetComponent<InteractableObj>().id == 0){
                        interactable0 = true;
                        mainCamera.enabled = false;
                        interactCamera0.enabled = true;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        whitePoint.SetActive(false);
                    }
                }
            }
        }else{
            eText.SetActive(false);
        }
    }
    void LookAround()
    {
        float x = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime - y;
        
            y2 -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            y2 = Mathf.Clamp(y2, -90f, 90f);
        
        y = y2;

        bool yChanged = true;
        if(y > 0){
            if(Physics.Raycast(torchLightPos.position, torchLightPos.forward, 0.02f)){
                yChanged = false;
            }
        }else{
            if(Physics.Raycast(torchLightPos.position, -torchLightPos.forward, 0.02f)){
                yChanged = false;
            }
        }

        if(x>0){
            if(Physics.Raycast(torchLightPos.position, torchLightPos.right, 0.01f)){
                x = 0;
            }
        }else {
            if(Physics.Raycast(torchLightPos.position, -torchLightPos.right, 0.01f)){
                x = 0;
            }
        }

        if(yChanged == true){
            mainCamera.gameObject.transform.localRotation = Quaternion.Euler(y2 - start, 0f, 0f);
        }
        mainCamera.gameObject.transform.parent.Rotate(Vector3.up * x);
    }
    void Movement()
    {
        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, ground);
        if(onGround == false){
            onGround = Physics.CheckSphere(groundCheck.position, groundDistance, ground2);
        }

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

        if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)){
            controller.Move(movementVector * speed *  Time.deltaTime);
        }else{
            movementVector.x=0;
            movementVector.y=0;
        }
    }
    void LeaveRoom()
    {
        _SceneChanger.sceneChange("Loading - Copy");
    }
    void Torch()
    {
        torch = !torch;
        print(torch);
        animator1.SetBool("b", true);
        animator1.SetBool("Torch", torch);
        print(animator1.GetBool("Torch"));
    }
    public void SavePlayer ()
    {
        SaveSystem.SavePlayer(this, inputField.text, confirmObj);
        inputField.transform.parent.gameObject.SetActive(false);
        if(File.Exists(Application.persistentDataPath + "/saves/" + inputField.text + ".fun")){
            print("yeah");
        }
    }
    public void ConfirmSavePlayer ()
    {
        SaveSystem.SavePlayer(this, inputField.text);
        confirmObj.transform.parent.gameObject.SetActive(false);
    }
    public void Restart ()
    {
        SceneManager.LoadScene("Loading");
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