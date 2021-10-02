using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto1
{
    public class WarriorConnection : MonoBehaviour
    {
        [SerializeField] public WarriorEnemy enemy;

        private void Start()
        {
            if (enemy == null)
                enemy = gameObject.GetComponentInParent<WarriorEnemy>();
        }

        public float GetDistanceToTarget(Vector2 position)
        {
            return enemy.GetDistanceToTarget(position);
        }

        public void Attack()
        {
            enemy.Attack();
        }
    }
}