using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : StateMachineBehaviour
{
 
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("HitRight");
        animator.ResetTrigger("HitLeft");
    }
    
}
