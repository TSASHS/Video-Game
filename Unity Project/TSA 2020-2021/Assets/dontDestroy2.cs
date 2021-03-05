using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroy2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
         GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
        if(objs.Length > 1){
            if(objs[0] == this){
                Destroy(objs[1]);
            }else{
                Destroy(objs[0]);
            }
        }
        DontDestroyOnLoad(this);
    }
}
