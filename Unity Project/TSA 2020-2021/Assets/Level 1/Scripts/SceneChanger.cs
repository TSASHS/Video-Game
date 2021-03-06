using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class SceneChanger : MonoBehaviour
{
    public string [] saveFiles;
    public Button loadGameButton;
    public GameObject nameDisplay;
    public int i = 1;
    public GameObject Panel;
    public void sceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
