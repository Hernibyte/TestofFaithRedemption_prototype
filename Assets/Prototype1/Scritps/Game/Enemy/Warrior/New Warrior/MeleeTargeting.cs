using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto1
{
    public class MeleeTargeting : StateMachineBehaviour
    {
        WarriorConnection thisEnemy;
    
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            thisEnemy = animator.GetComponent<WarriorConnection>();
        }
    
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (thisEnemy.GetDistanceToTarget(thisEnemy.enemy.target.transform.position) < thisEnemy.ValueToTarget())
            {
                animator.SetTrigger("targetPlayer");
            }
        }
    
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger("targetPlayer");
        }
    }
}