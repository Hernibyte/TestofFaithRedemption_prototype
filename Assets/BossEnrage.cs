using UnityEngine;

namespace Proto1
{
    public class BossEnrage : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            BossEnemy bossRef = animator.GetComponent<BossEnemy>();

            if (bossRef != null)
                bossRef.isInvulnerable = true;
        }
    
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            BossEnemy bossRef = animator.GetComponent<BossEnemy>();

            if (bossRef != null)
                bossRef.isInvulnerable = false;
        }
    }
}