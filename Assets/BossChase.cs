using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto1
{
    public class BossChase : StateMachineBehaviour
    {
        float speedBoss;

        BossEnemy boss;

        GameObject player;
        Rigidbody2D rb;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss = animator.GetComponent<BossEnemy>();
            rb = boss.rb;
            speedBoss = boss.bossSpeed;

            player = boss.target;
        }
    
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (player == null)
                return;

            Vector2 target = new Vector2(player.transform.position.x, player.transform.position.y/* * 0.5f*/);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speedBoss * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if(boss.DistanceToTarget(player.transform.position) <= boss.attackRange)
            {
                animator.SetTrigger("Attack");
            }
        }
    
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
                animator.ResetTrigger("Attack");
        }
    }
}