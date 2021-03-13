using UnityEngine;
using UnityEngine.SceneManagement;

public class TreasureRoomScript : MonoBehaviour
{
    public float lightIntensity  = 3;
    public float tick = 0;
    public float tickEnd;
    public float creditsStart;
    public float spaceTick = 0; 
    public float popuptime = 120;
    private bool ended = false;
    public Light[] lights;
    public Animator creditsAnim, creditsEndAnim;
    void Awake ()
    {
        GameObject [] array = GameObject.FindGameObjectsWithTag("music");
        foreach(GameObject obj in array){
            Destroy(obj);
        }
        Time.timeScale = 1;
    }
    void Update ()
    {
        lightIntensity += Random.Range(-0.05f, 0.05f);
        lightIntensity = Mathf.Clamp(lightIntensity, 2, 4);
        foreach(Light l in lights){
            l.intensity = lightIntensity;
        }
        tick += Time.deltaTime;
        if(tick > creditsStart){
            if(Input.GetKey(KeyCode.Space)){
                spaceTick += Time.deltaTime;
            }else{
                spaceTick = 0;
            }
            if(spaceTick >= 2.5){
                creditsAnim.CrossFade("State", 0f);
                creditsEndAnim.CrossFade("Credits2", 0f);
                tick = popuptime;
            }
        }
        if(tick >= popuptime && ended == false){
            creditsEndAnim.CrossFade("Credits2", 0f);
            ended = true;
        }
        if(tick >= tickEnd){
            SceneManager.LoadScene("Start Screen");
        }
    }
}