using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsScript : MonoBehaviour
{
    public GameObject qualityTextObj;
    public GameObject resolutionTextObj;
    TextMeshProUGUI qualityText;
    TextMeshProUGUI resolutionText;
    int qualityLevel = 0;
    float resoltuionLevel = 0;
    List<string> qualityList = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        qualityText = qualityTextObj.GetComponent<TextMeshProUGUI>();
        resolutionText=resolutionTextObj.GetComponent<TextMeshProUGUI>();
        qualityList.Add("Very Low");
        qualityList.Add("Low");
        qualityList.Add("Medium");
        qualityList.Add("High");
        qualityList.Add("Ultra High");
        qualityText.text = qualityList[qualityLevel];
        resolutionText.text = "1280 - 720";
        //640*480, 1920 * 1080, 1280*720
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IncreaseQuality()
    {
        qualityLevel += 1;
        qualityLevel = Mathf.Clamp(qualityLevel, 0,4);
        qualityText.text = qualityList[qualityLevel];
    }
    public void DecreaseQuality()
    {
        qualityLevel -= 1;
        qualityLevel = Mathf.Clamp(qualityLevel, 0,4);
        qualityText.text = qualityList[qualityLevel];
    }
    public void IncreaseResolution()
    {
        resoltuionLevel += 1;
        resoltuionLevel = Mathf.Clamp(resoltuionLevel, 0, 2);
        if(resoltuionLevel == 1){
            resolutionText.text = "1280 - 720";
        }else{
            resolutionText.text = "1920 - 1080";
        }
    }
    public void DecreaseResolution()
    {
        resoltuionLevel -= 1;
        resoltuionLevel = Mathf.Clamp(resoltuionLevel, 0, 2);
        if(resoltuionLevel == 1){
            resolutionText.text = "1280 - 720";
        }else{
            resolutionText.text = "640*480";
        }
    }
}
