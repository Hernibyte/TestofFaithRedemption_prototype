using UnityEngine;

namespace Proto1
{
    public class AttackBehaviour : MonoBehaviour
    {
        [SerializeField] PlayerAttack playerAttack;

        public void MeleeDamage()
        {
            playerAttack.MeleeAttack();
        }
    }
}