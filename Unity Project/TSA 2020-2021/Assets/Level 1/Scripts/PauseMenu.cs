using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu, cursor, tutorialsprite, settingsMenu;
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
                tutorialsprite.SetActive(false);
            }else{
                unpause();
                Cursor.lockState = CursorLockMode.Locked;
                cursor.SetActive(true);
                tutorialsprite.SetActive(true);
            }
            if(settingsMenu.activeSelf == true){
                settingsMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                cursor.SetActive(true);
                tutorialsprite.SetActive(true);
            }
        }
    }
    public void unpause()
    {
        Time.timeScale = 1f;
    }
}
