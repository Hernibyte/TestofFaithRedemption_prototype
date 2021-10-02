using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto1
{
    public class WarriorChase : StateMachineBehaviour
    {
        float speed;
        Rigidbody2D rb;

        WarriorConnection enemyRef;
        GameObject target;
    
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            enemyRef = animator.gameObject.GetComponent<WarriorConnection>();

            if (enemyRef == null)
            {
                Debug.LogWarning("Warrior enemy at WarriorChase is Missing!___NullReference Exception.");
                return;
            }

            speed = enemyRef.enemy.speed;
            rb = enemyRef.enemy.rb;
            target = enemyRef.enemy.target;
        }
    
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (target == null)
                return;

            Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
            Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);

            if(enemyRef.GetDistanceToTarget(targetPosition) <= enemyRef.enemy.range)
            {
                animator.SetTrigger("attack");
            }
        }
    
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger("attack");
        }
    }
}