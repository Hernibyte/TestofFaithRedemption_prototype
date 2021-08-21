using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto1
{
    public class Enemy : MonoBehaviour, IHittable
    {
        [SerializeField] Animator enemyAnimator;
        [SerializeField] SpriteRenderer spriteEnemy;
        [SerializeField] Rigidbody2D rig;
        [SerializeField] float enemySpeed;

        [SerializeField] GameObject target;
        public float distanceToTarget;
        public float maxNearDistance;

        [System.Serializable]
        public enum STATENEMY
        {
            Attacking,
            Following
        }
        public STATENEMY enemyState;

        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    
        void Update()
        {
            switch (enemyState)
            {
                case STATENEMY.Attacking:

                    Attack();

                    break;
                case STATENEMY.Following:

                    GoToTarget();

                    break;
            }
        }

        void GoToTarget()
        {
            Debug.Log("Distancia:" + Vector2.Distance(transform.position, target.transform.position).ToString());

            if(Vector2.Distance(transform.position, target.transform.position) < maxNearDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, enemySpeed * Time.deltaTime);
            }
        }

        void Attack()
        {

        }

        public void Hit()
        {
            Destroy(gameObject);
        }
    }
}
