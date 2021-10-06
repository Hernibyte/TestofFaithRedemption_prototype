using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto1
{
    public class Comb_System : MonoBehaviour
    {
        [SerializeField] List<AttackState> attacks;
        [SerializeField] public int attackIteration;
        [SerializeField] public float resetCombSystem;
        [SerializeField] public float timePerIteration;

        private int flagStartCombo = -1;

        public void CombSystem(Animator playerAnimator, PlayerMovement playerMove)
        {
            if (playerAnimator == null && playerMove == null)
                return;

            if(attackIteration == flagStartCombo)
            {
                InteedAttack(playerAnimator);
            }
            else
            {
                if(timePerIteration < resetCombSystem)
                {
                    timePerIteration += Time.fixedDeltaTime;

                    if(attacks[attackIteration].durationAtk < attacks[attackIteration].atkSpeed)
                    {
                        if(attacks[attackIteration].triggered)
                            attacks[attackIteration].durationAtk += Time.deltaTime;

                        if (attackIteration == attacks.Count-1)
                            playerMove.DoAfterImageEffect();
                    }
                    else
                    {
                        if(attackIteration + 1 < attacks.Count)
                        {
                            if (Input.GetKeyDown(KeyCode.Mouse0) && !attacks[attackIteration + 1].triggered)
                            {
                                attacks[attackIteration].durationAtk = 0;
                                NextCombo();
                                playerAnimator.SetTrigger("attack");
                            }
                        }
                    }
                }
                else
                {
                    ResetCombo();
                }
            }
        }
        public void InteedAttack(Animator playerAnimator)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                NextCombo();
                playerAnimator.SetTrigger("attack");
            }
        }
        public AttackState MakeAttack()
        {
            if (attackIteration == flagStartCombo || attackIteration >= attacks.Count)
                return null;

            if (attacks[attackIteration] != null)
                return attacks[attackIteration];
            else
                return null;
        }
        public void NextCombo()
        {
            if (attackIteration < attacks.Count-1)
            {
                attackIteration++;
                attacks[attackIteration].triggered = true;
                timePerIteration = 0;
            }
            else
            {
                ResetCombo();
            }
        }
        public void ResetCombo()
        {
            for (int i = 0; i < attacks.Count; i++)
            {
                attacks[i].durationAtk = 0;
                attacks[i].triggered = false;
            }
            attackIteration = flagStartCombo;
        }
    }
}