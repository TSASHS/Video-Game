using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public static class SaveSystem
{
    public static void SavePlayer (Main level2Main, string fileName, GameObject confirmObj)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if(!Directory.Exists(Application.persistentDataPath + "/saves")){
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }
        string path = Application.persistentDataPath + "/saves/" + fileName +".fun";
        if (File.Exists(path)){
            confirmObj.SetActive(true);
        }else{
            FileStream stream = new FileStream(path, FileMode.Create);
            PlayerData data;
            data = new PlayerData(level2Main);
            formatter.Serialize(stream, data);
            stream.Close();
            confirmObj.transform.parent.gameObject.SetActive(false);
        }
    }
        public static void SavePlayer (TutorialMainScript tutorialMain, string fileName, GameObject confirmObj)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if(!Directory.Exists(Application.persistentDataPath + "/saves")){
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }
        string path = Application.persistentDataPath + "/saves/" + fileName +".fun";
        if (File.Exists(path)){
            confirmObj.SetActive(true);
        }else{
            FileStream stream = new FileStream(path, FileMode.Create);
            PlayerData data;
            data = new PlayerData(tutorialMain);
            formatter.Serialize(stream, data);
            stream.Close();
            confirmObj.transform.parent.gameObject.SetActive(false);        
        }
    }
    public static void SavePlayer (TutorialMainScript tutorialMain, string fileName){
        BinaryFormatter formatter = new BinaryFormatter();
        if(!Directory.Exists(Application.persistentDataPath + "/saves")){
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }
        string path = Application.persistentDataPath + "/saves/" + fileName +".fun";
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data;
        data = new PlayerData(tutorialMain);
        formatter.Serialize(stream, data);
        stream.Close();
    }
        public static void SavePlayer (Main level2Main, string fileName){
        BinaryFormatter formatter = new BinaryFormatter();
        if(!Directory.Exists(Application.persistentDataPath + "/saves")){
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }
        string path = Application.persistentDataPath + "/saves/" + fileName +".fun";
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data;
        data = new PlayerData(level2Main);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static PlayerData LoadPlayer (string text)
    {
        string path = Application.persistentDataPath + "/saves/" + text + ".fun";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }else{
            return null;
        }
    }
}
