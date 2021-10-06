using UnityEngine;

namespace Proto1
{
    public class AttackState : MonoBehaviour
    {
        [SerializeField]
        public enum AttackType { Melee, Ranged}
        public AttackType typeAttack;

        [Header("ATTACK NEEDS")]
        [SerializeField] public LayerMask enemyLayer;
        [SerializeField] public Transform meleeAttackPoint;
        [Space(20)]
        [Header("ATTACK STATS")]
        public int damage;
        public float knockBack;
        public float range;
        public float impulseAttack;
        public float atkSpeed;
        public float durationAtk;

        public bool triggered;
    }
}