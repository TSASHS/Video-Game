using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMainScript : MonoBehaviour
{
    public bool interactable0;
    public GameObject eText;
    public LayerMask interactableLayer, cubes;
    public Camera mainCamera, interactCamera0;

    public bool challengeCompleted = false;
    public Animator animator;
    public List<GameObject> cubeList = new List<GameObject>();
    public List<Image> circleList = new List<Image>();
    private bool challenge = false;
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
    public float groundDistance = 1.2f;
    public LayerMask ground;
    public bool onGround = true;
    public Color32 circleColor = new Color32(41, 108, 164, 255);
    private Color circleColor1;
    private GameObject blueCube;
    private bool moving;
    // Start is called before the first frame update
    // Update is called once per frame
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        start = -Input.GetAxis("Mouse Y");
        circleColor1 = circleColor;
        foreach(GameObject cube in cubeList){
            cube.GetComponent<Renderer>().material.shader = Shader.Find("Shader Graphs/Outline");
            cube.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.black);
            cube.GetComponent<Puzzle1Cube>().node.pos = cube.transform.position;
        }
    }
    void Update()
    {
        eTextFunc();
        Challenge1();
        if(challenge != true){
            LookAround();
            Movement();
        }
    }
    void eTextFunc (){
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 2f,interactableLayer)){
            if(hit.transform.gameObject.GetComponent<InteractableObj>().interactable == true && interactable0 == false){
                eText.SetActive(true);
                if(Input.GetKeyDown(KeyCode.E)){
                    if(hit.transform.gameObject.GetComponent<InteractableObj>().id == 0){
                        interactable0 = true;
                        mainCamera.enabled = false;
                        interactCamera0.enabled = true;
                    }
                }
            }
        }else{
            eText.SetActive(false);
        }
    }
    void Challenge1()
    {
        challenge = interactable0;
        if(challenge == true){
            Cursor.lockState = CursorLockMode.None;
            eObj.SetActive(false);
            whitePoint.SetActive(false);
            if(Input.GetMouseButtonDown(0) && moving == false){
                RaycastHit hit;
                Ray ray = interactCamera0.ScreenPointToRay(Input.mousePosition);
                cubeList[8].GetComponent<Puzzle1Cube>().node.image.color = Color.clear;                
                if(blueCube != null){
                    blueCube.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.black);
                }
                if(Physics.Raycast(ray, out hit, 10000f, cubes)){
                    print(1);
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
                            print("yesss");
                            cubeList[8].GetComponent<Puzzle1Cube>().node.image.color = circleColor;
                        }              
                    }else{
                        blueCube = null;
                    }
                }
            }
            if(moving == true)
            {
                Vector3 oldPos = blueCube.transform.position;
                Vector3 newPos = blueCube.GetComponent<Puzzle1Cube>().node.pos;
                if(oldPos != newPos){
                    if(oldPos.x > newPos.x){
                        print(1);
                        blueCube.transform.position -= Vector3.right * 1.75f * Time.deltaTime;
                        blueCube.transform.position = new Vector3(Mathf.Clamp(blueCube.transform.position.x, newPos.x, oldPos.x), blueCube.transform.position.y, oldPos.z);
                    }else if (oldPos.x < newPos.x){
                        blueCube.transform.position += Vector3.right * 1.75f * Time.deltaTime;
                        blueCube.transform.position = new Vector3(Mathf.Clamp(blueCube.transform.position.x, oldPos.x, newPos.x), blueCube.transform.position.y, oldPos.z);
                    }
                    if(oldPos.y > newPos.y){
                        print(3);
                        blueCube.transform.position -= Vector3.up * 1.75f * Time.deltaTime;
                        blueCube.transform.position = new Vector3(blueCube.transform.position.x, Mathf.Clamp(blueCube.transform.position.y, newPos.y, oldPos.y), oldPos.z);                        
                    }else if (oldPos.y < newPos.y){
                        print(4);
                        blueCube.transform.position += Vector3.up * 1.75f * Time.deltaTime;
                        blueCube.transform.position = new Vector3(blueCube.transform.position.x, Mathf.Clamp(blueCube.transform.position.y, oldPos.y, newPos.y), oldPos.z);
                    }
                }else{
                    moving = false;
                    blueCube = null;
                }
            }
        }
    }
    void LookAround()
    {
        float x = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        y2 -= y;
        y2 = Mathf.Clamp(y2, -90f, 90f);

        mainCamera.gameObject.transform.localRotation = Quaternion.Euler(y2 - start, 0f, 0f);
        mainCamera.gameObject.transform.parent.Rotate(Vector3.up * x);
    }
    void Movement()
    {
        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, ground);

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 movementVector = controller.transform.right * x + controller.transform.forward * y;

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
//afe