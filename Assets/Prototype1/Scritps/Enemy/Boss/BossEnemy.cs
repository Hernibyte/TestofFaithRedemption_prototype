﻿using UnityEngine;

namespace Proto1
{
    public class BossEnemy : MonoBehaviour, IHittable
    {
        [Header("BOSS NEEDS")]
        [Space(20)]
        [SerializeField] LayerMask playerLayer;
        [SerializeField] SpriteRenderer bossSprite;
        [SerializeField] public GameObject target;
        [SerializeField] public Rigidbody2D rb;

        [Header("BOSS STATS")]
        [Space(20)]
        [SerializeField] public float bossSpeed;
        [SerializeField] public float attackRange;
        [SerializeField] public int bossDamage;
        [SerializeField] public float bossKnockBack;
        [SerializeField] public float bossActualHP;
        [SerializeField] public float bossMAX_HP;

        [Header("NO TOCAR")]
        [Space(20)]
        [SerializeField] float colliderOffsetXRight;
        [SerializeField] float colliderOffsetXLeft;

        BoxCollider2D bossCollider;

        public delegate void UpdateBossUIData(int amountDamage);
        public UpdateBossUIData updateUIData;

        void Start()
        {
            bossCollider = gameObject.GetComponent<BoxCollider2D>();

            bossActualHP = bossMAX_HP;

            target = GameObject.FindGameObjectWithTag("Player");
        }
        void Update()
        {
            if (target == null)
                return;


            if (transform.position.x < target.transform.position.x)
            {
                bossCollider.offset = new Vector2(colliderOffsetXRight, 0);
                bossSprite.flipX = false;
            }
            else
            {
                bossCollider.offset = new Vector2(colliderOffsetXLeft, 0);
                bossSprite.flipX = true;
            }
        }
        public float DistanceToTarget(Vector2 target)
        {
            return Vector2.Distance(rb.position, target);
        }
        public void Attack()
        {
            Vector2 attackPosition = new Vector2(rb.position.x, rb.position.y);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPosition, attackRange, playerLayer);
            foreach (Collider2D collider in colliders)
            {
                IHittable hittable = collider.GetComponent<IHittable>();
                if (hittable != null)
                {
                    hittable.Hit(bossDamage, bossKnockBack, transform.position);
                }
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0f), attackRange);
        }
        public void Hit(int damage, float kncokBack, Vector2 posAttacker)
        {
            if (bossActualHP > 0)
            {
                bossActualHP -= damage;

                updateUIData?.Invoke(damage);
            }

            if(bossActualHP < 0)
            {
                bossActualHP = 0;
                Die();
            }
        }
        public void Die()
        {
            Destroy(gameObject);
        }
    }
}