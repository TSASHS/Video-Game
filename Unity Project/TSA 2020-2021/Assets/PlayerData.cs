using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData 
{
    public int level;
    public float[] position = new float [3];
    public float[] rotation = new float [4];
    public int tutorialStage = 0;
    public string[] buttonsClicked;
    public Dictionary<int, float[]> firstLevelBoxPositions = new Dictionary<int, float[]>();
    public Dictionary<int, bool> levelTwoPositions = new Dictionary <int, bool>();
    public Dictionary<int, bool> leverPositions = new Dictionary<int, bool>();
    public bool torchState;
    public bool tutorialDoor;
    public bool tutorialDoor2;
    public bool level2Floor;
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
        level2Floor = level2Main.challengeCompleted;
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
                buttonsClicked[buttonsClicked.Length - 1] = "w";
            }
            if(tutorialMain.aClicked == true){
                buttonsClicked[buttonsClicked.Length - 1] = "a";
            }
            if(tutorialMain.sClicked == true){
                buttonsClicked[buttonsClicked.Length - 1] = "s";
            }
            if(tutorialMain.dClicked == true){
                buttonsClicked[buttonsClicked.Length - 1] = "d";
            }
        }
        for(int i = 0; i < tutorialMain.cubeList.Count; i ++){
            firstLevelBoxPositions.Add(i, new float[3]);
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
        tutorialDoor2 = tutorialMain.challengeCompleted;
        tutorialDoor = tutorialMain.enteredRoom;
    }
}