using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData : MonoBehaviour
{
    public int level;
    public float[] position;
    public float[] rotation;
    public int tutorialStage = 0;
    public string[] buttonsClicked;
    public Dictionary<int, float[]> firstLevelBoxPositions;
    public Dictionary<int, bool> levelTwoPositions;
    public Dictionary<int, bool> leverPositions;
    public bool torchState;

    public PlayerData (Main level2Main){
        level = SceneManager.GetActiveScene().buildIndex;
        Vector3 transform;
        Quaternion rotate;
        transform = level2Main.controller.gameObject.transform.position;
        rotate = level2Main.controller.gameObject.transform.rotation;
        for(int i = 0; i < 5; i++){
            levelTwoPositions[i] = level2Main.onOffList[i];
            leverPositions[i] = level2Main.leverList[i];
        }
        position[0] = transform.x;
        position[1] = transform.y;
        position[2] = transform.z;
        rotation[0] = rotate.w;
        rotation[1] = rotate.x;
        rotation[2] = rotate.y;
        rotation[3] = rotate.z;
    }
        public PlayerData (TutorialMainScript tutorialMain){
        level = SceneManager.GetActiveScene().buildIndex;
        Vector3 transform;
        Quaternion rotate;
        transform = tutorialMain.controller.gameObject.transform.position;
        rotate = tutorialMain.controller.gameObject.transform.rotation;
        tutorialStage = tutorialMain.tutorialStage;
        if(tutorialStage == 2){
            if(tutorialMain.wClicked == true){
                buttonsClicked[buttonsClicked.Length] = "w";
            }
            if(tutorialMain.aClicked == true){
                buttonsClicked[buttonsClicked.Length] = "a";
            }
            if(tutorialMain.sClicked == true){
                buttonsClicked[buttonsClicked.Length] = "s";
            }
            if(tutorialMain.dClicked == true){
                buttonsClicked[buttonsClicked.Length] = "d";
            }
        }
        for(int i = 0; i < tutorialMain.cubeList.Count; i ++){
            firstLevelBoxPositions[i][0] = tutorialMain.cubeList[i].transform.position.x;
            firstLevelBoxPositions[i][1] = tutorialMain.cubeList[i].transform.position.y;
            firstLevelBoxPositions[i][2] = tutorialMain.cubeList[i].transform.position.z;
        }
        position[0] = transform.x;
        position[1] = transform.y;
        position[2] = transform.z;
        rotation[0] = rotate.w;
        rotation[1] = rotate.x;
        rotation[2] = rotate.y;
        rotation[3] = rotate.z;
    }
}