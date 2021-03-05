using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
        print (objs.Length);
        if(objs.Length > 1){
            print ("yes");
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }
}
