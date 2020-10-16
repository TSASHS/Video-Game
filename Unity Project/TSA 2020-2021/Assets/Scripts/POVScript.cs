using UnityEngine;

public class POVScript : MonoBehaviour
{
    public float mouseSensitivity = 120f;
    float y2 = 0;
    float start;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        start = -Input.GetAxis("Mouse Y");
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        y2 -= y;
        y2 = Mathf.Clamp(y2, -90f, 90f);

        transform.localRotation = Quaternion.Euler(y2 - start, 0f, 0f);
        transform.parent.Rotate(Vector3.up * x);
    }
}
