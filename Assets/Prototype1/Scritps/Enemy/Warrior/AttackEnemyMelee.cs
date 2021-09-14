using UnityEngine;

namespace Proto1
{
    public class AttackEnemyMelee : MonoBehaviour
    {
        [SerializeField] MeleeEnemy enemy;
        void MeleeEnemyAttack()
        {
            enemy.MeleeAttack();
        }
    }
}