using UnityEngine;
using System.Collections;

public class EndGameCollision : MonoBehaviour
{
    public Main _main;
    void OnTriggerEnter(Collider col)
    {
        print (1);
        if(col.gameObject.tag == "Player"){
            _main.LeaveRoom();
        }
    }   
}