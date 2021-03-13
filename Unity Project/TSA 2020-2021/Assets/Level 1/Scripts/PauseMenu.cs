using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu, cursor, tutorialsprite, settingsMenu;
    public bool yea = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if(pauseMenu.activeSelf == true){
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                cursor.SetActive(false);
                if(yea == true){
                    tutorialsprite.SetActive(false);
                }
            }else{
                if(settingsMenu.activeSelf == true){
                    print(2);
                    settingsMenu.SetActive(false);
                    pauseMenu.SetActive(true);
                }else{
                    print(1);
                    unpause();
                }
            }

        }
    }
    public void unpause()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        cursor.SetActive(true);
        if(yea == true){
            tutorialsprite.SetActive(true);
        }   
    }
}
