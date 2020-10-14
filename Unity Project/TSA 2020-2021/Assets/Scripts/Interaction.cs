using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    public GameObject eText;
    public LayerMask interactableLayer;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),out hit, 4f,interactableLayer)){
            if(hit.transform.gameObject.GetComponent<InteractableObj>().interactable == true){
                eText.SetActive(true);
                if(Input.GetKeyDown(KeyCode.E)){
                    hit.transform.gameObject.GetComponent<Animator>().SetBool("Animation 1 Condition", true);
                    hit.transform.gameObject.GetComponent<InteractableObj>().interactable = false;
                }
            }
        }else{
            eText.SetActive(false);
        }
    }
}
