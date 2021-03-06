using UnityEngine;
using UnityEngine.SceneManagement;
public class Loading : MonoBehaviour
{
    public string sceneName;
    public float i = 0;
    // Start is called before the first frame update
    void Update()
    {
        i += 1;
        if(i >= 2){
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }   
    }
}
