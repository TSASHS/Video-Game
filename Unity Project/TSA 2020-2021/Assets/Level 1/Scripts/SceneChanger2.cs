using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class SceneChanger2 : MonoBehaviour
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
    void Start()
    {
        GetLoadFiles();
    }
    void Update()
    {
        if(saveFiles.Length > 0){
            loadGameButton.enabled = true;
            for(int i = 0; i < nameDisplay.transform.parent.childCount; i++){
                string s = saveFiles[i];
                string[] sArr;
                sArr = s.Split('\\');
                sArr = sArr[1].Split('.');
                nameDisplay.transform.parent.GetChild(i).GetComponent<TextMeshProUGUI>().text = sArr[0];
            }
        }else{
            loadGameButton.enabled = false;
        }
        while(i != saveFiles.Length && saveFiles.Length != 0){
            if(i > saveFiles.Length){
                Destroy(nameDisplay.transform.parent.GetChild(nameDisplay.transform.parent.childCount - 1));
                i -= 1;
            }else{
                GameObject obj = Instantiate(nameDisplay, new Vector3(nameDisplay.transform.position.x, nameDisplay.transform.position.y - (46*nameDisplay.transform.parent.childCount),nameDisplay.transform.position.z), new Quaternion(0,0,0,0));
                obj.transform.parent = nameDisplay.transform.parent;
                obj.transform.localScale = new Vector3(1,1,1);
                i++;
            }
        }
        if(saveFiles.Length > 0){
            loadGameButton.enabled = true;
            for(int i = 0; i < nameDisplay.transform.parent.childCount; i++){
                string s = saveFiles[i];
                string[] sArr;
                sArr = s.Split('\\');
                sArr = sArr[1].Split('.');
                nameDisplay.transform.parent.GetChild(i).GetComponent<TextMeshProUGUI>().text = sArr[0];
            }
        }else{
            loadGameButton.enabled = false;
        }
    }
    public void GetLoadFiles()
    {
        if(Directory.Exists(Application.persistentDataPath + "/saves")){
            saveFiles = Directory.GetFiles(Application.persistentDataPath + "/saves");
        }
    }
    public void LoadGame(TextMeshProUGUI text)
    {
        PlayerData data = SaveSystem.LoadPlayer(text);
    }
}
