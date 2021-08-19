using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto1
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] LayerMask enemyLayer;
        [HideInInspector] public float horizontalAttack;
        [HideInInspector] public float verticalAttack;
        [SerializeField] Animator attack;

        void Start()
        {
            
        }
    
        void Update()
        {
            Attack();
        }

        void Attack()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector2 attackPosition= new Vector2(transform.position.x + horizontalAttack, transform.position.y + verticalAttack);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPosition, 0.4f, enemyLayer);
                foreach (Collider2D collider in colliders)
                {
                    IHittable hittable = collider.GetComponent<IHittable>();
                    if(hittable != null)
                    {
                        hittable.Hit();
                    }
                }
                attack.SetTrigger("attack");
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(new Vector3(transform.position.x + horizontalAttack, transform.position.y + verticalAttack, 0f), 0.4f);
        }
    }
}