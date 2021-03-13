using UnityEngine;
using UnityEngine.SceneManagement;
public class collisionClass : MonoBehaviour 
{
    void OnTriggerEnter(Collider col)
    {
        SceneManager.LoadScene("Treasure Room");
    }
}
