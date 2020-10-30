using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeScript : MonoBehaviour
{
    public bool challengeCompleted = false;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if(a)
        {
            SetChallengeComplete();
        }
        */
        if(Input.GetKeyDown(KeyCode.C)){
            SetChallengeComplete();
        }
    }
    void SetChallengeComplete ()
    {
        challengeCompleted = true;
        animator.SetBool("ChallengeCompleted", true);
    }
}
