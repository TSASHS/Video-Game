using UnityEngine;
using TMPro;

public class QualityText : MonoBehaviour
{   
    public TextMeshProUGUI text;
    private string [] names;
    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<TextMeshProUGUI>();
        names = QualitySettings.names;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = names[QualitySettings.GetQualityLevel()]; 
    }
    public void IncreaeLevel()
    {
        QualitySettings.IncreaseLevel();
    }
    public void DecreaseLevel()
    {
        QualitySettings.DecreaseLevel();
    }
}
