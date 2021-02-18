using UnityEngine;
using UnityEngine.SceneManagement;
public class Loading : MonoBehaviour
{
    public string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
