using UnityEngine;
using UnityEngine.SceneManagement;
public class collisionClass : MonoBehaviour 
{
    void OnTriggerEnter()
    {
        SceneManager.LoadScene("Treasure Room");
    }
}
