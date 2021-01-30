using UnityEngine;

public class Main : MonoBehaviour
{
    public Camera mainCamera;
    public CharacterController controller;
    public float mouseSensitivity;
    private float tick = 0;
    private float y;

    void Start ()
    {

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

        if(x > 0){
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
    void Movement ()
    {

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
    void LeaveRoom()
    {
        _SceneChanger.sceneChange("Level 3");
    }
}