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
    public float[] cameraRotation = new float [4];
    public int tutorialStage = 0;
    public string[] buttonsClicked = new string[4];
    public List<float> firstLevelBoxPositionsx = new List<float>();
    public List<float> firstLevelBoxPositionsy = new List<float>();

    public List<float> firstLevelBoxPositionsid = new List<float>();

    public List <bool> levelTwoPositions = new List < bool>();
    public List<bool> leverPositions = new List<bool>();
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
            levelTwoPositions.Add(new bool());
            levelTwoPositions[i] = level2Main.onOffList[i];
            leverPositions.Add(new bool());
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
    public PlayerData (LevelMain levelMain){
        level = SceneManager.GetActiveScene().buildIndex;
        Vector3 transform;
        Quaternion rotate;
        transform = levelMain.controller.gameObject.transform.position;
        rotate = levelMain.controller.gameObject.transform.rotation;
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
        Quaternion cameraRotate;
        transform = tutorialMain.controller.gameObject.transform.position;
        rotate = tutorialMain.controller.gameObject.transform.rotation;
        cameraRotate = tutorialMain.player.transform.GetChild(2).rotation;
        tutorialStage = tutorialMain.tutorialStage;
        if(tutorialStage == 2){
            if(tutorialMain.wClicked == true){
                buttonsClicked[0] = "w";
            }
            if(tutorialMain.aClicked == true){
                buttonsClicked[1] = "a";
            }
            if(tutorialMain.sClicked == true){
                buttonsClicked[2] = "s";
            }
            if(tutorialMain.dClicked == true){
                buttonsClicked[3] = "d";
            }
        }
        for(int i = 0; i < tutorialMain.cubeList.Count; i ++){
            firstLevelBoxPositionsx.Add(new float());
            firstLevelBoxPositionsy.Add(new float());
            firstLevelBoxPositionsid.Add(new float());
            firstLevelBoxPositionsx[i] = tutorialMain.cubeList[i].transform.position.x;
            firstLevelBoxPositionsy[i] = tutorialMain.cubeList[i].transform.position.y;
            firstLevelBoxPositionsid[i] = tutorialMain.cubeList[i].GetComponent<Puzzle1Cube>().node.id;
        }
        position[0] = transform.x;
        position[1] = transform.y;
        position[2] = transform.z;
        rotation[0] = rotate.w;
        rotation[1] = rotate.x;
        rotation[2] = rotate.y;
        rotation[3] = rotate.z;
        rotation[0] = rotate.w;
        rotation[1] = rotate.x;
        rotation[2] = rotate.y;
        rotation[3] = rotate.z;
        cameraRotation[0] = cameraRotate.w;
        cameraRotation[1] = cameraRotate.x;
        cameraRotation[2] = cameraRotate.y;
        cameraRotation[3] = cameraRotate.z;        
        tutorialDoor2 = tutorialMain.challengeCompleted;
        tutorialDoor = tutorialMain.enteredRoom;
    }
}