using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialSprite;
    public GameObject player, keys;
    GameObject playerCamera;
    TextMeshProUGUI tutorialText;
    int tutorialStage = 0;
    bool secondCountBool, wClicked, aClicked, dClicked, sClicked = false;
    public Image wSprite, aSprite, dSprite, sSprite;
    float secondCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = player.transform.GetChild(2).gameObject;
        tutorialText = tutorialSprite.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tutorialSprite.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        if(tutorialStage == 0){
            tutorialText.text = "Move the cursor to look around";
            if(Mathf.Abs(player.transform.rotation.y) > 5/360|| Mathf.Abs(playerCamera.transform.localRotation.x) > 5/360){
                secondCountBool = true;
                tutorialText.color = Color.green;
            }
        }
        if(tutorialStage == 1){
            keys.SetActive(true);
            tutorialText.text = "Use the                    keys to move";
            if(Input.GetKeyDown(KeyCode.W)){
                wClicked = true;
                wSprite.color = Color.green;
            }
            if(Input.GetKeyDown(KeyCode.A)){
                aClicked = true;
                aSprite.color = Color.green;                
            }
            if(Input.GetKeyDown(KeyCode.D)){
                dClicked = true;
                dSprite.color = Color.green;
            }
            if(Input.GetKeyDown(KeyCode.S)){
                sClicked = true;
                sSprite.color = Color.green;
            }
            if(wClicked == true && aClicked == true&& dClicked == true && sClicked == true){
                tutorialText.color = Color.green;
                secondCountBool = true;
            }
        }
        if(tutorialStage == 2){
            tutorialText.text = "Walk to the marked location";
        }
        if(secondCountBool == true){
            secondCount += Time.deltaTime;
            if(secondCount > 1){
                secondCountBool = false;
                tutorialText.color = Color.black;
                tutorialStage += 1;
                secondCount = 0;
                keys.SetActive(false);
            }
        }
    }
}
