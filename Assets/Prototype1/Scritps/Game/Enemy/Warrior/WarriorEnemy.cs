using UnityEngine;
using UnityEngine.Events;

namespace Proto1
{
    public class WarriorEnemy : MonoBehaviour, IHittable
    {
        public delegate void UpdateEnemyUIData(int amountDamage);
        public UpdateEnemyUIData updateUIData;
        public UnityEvent deathEvent;

        [Space(10)]
        [Header("MELEE ENEMY NEEDS")]
        [SerializeField] Rigidbody2D rb;
        [SerializeField] LayerMask playerLayer;
        [SerializeField] SpriteRenderer spriteEnemy;
        [SerializeField] GameObject slashEffect;
        [HideInInspector]public GameObject target;

        [Space(10)]
        [Header("MELEE ENEMY [ BASIC STATS ]")]
        [SerializeField] public float actualHP;
        [SerializeField] public float maxHP;
        [SerializeField] public float speed;
        [SerializeField] public int damage;
        [SerializeField] public float knockBack;
        [SerializeField] public float range;
        [Space(10)]
        [Header("MELEE ENEMY [ OTHER STATS ]")]
        [SerializeField] public bool stunned;
        [SerializeField] public AnimationCurve knockBackCurve;
        [SerializeField] public float t;

        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player");
            actualHP = maxHP;
        }

        private void Update()
        {
            if(stunned)
            {
                t += Time.fixedDeltaTime;

                if(knockBackCurve.Evaluate(t) < 1)
                {
                    if (rb.velocity.magnitude > Vector2.zero.magnitude)
                        rb.velocity -= new Vector2(2f * Time.fixedDeltaTime, 1.5f * Time.fixedDeltaTime);
                    else
                        rb.velocity = Vector2.zero;
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    stunned = false;
                    t = 0;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0f), range);
        }

        public float GetDistanceToTarget()
        {
            if (target == null)
                return 0f;

            return Vector2.Distance(transform.position, target.transform.position);
        }

        public void Attack()
        {
            Vector2 attackPos = new Vector2(rb.position.x, rb.position.y);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos, range, playerLayer);
            foreach(Collider2D colision in colliders)
            {
                IHittable hit = colision.GetComponent<IHittable>();
                if(hit != null)
                {
                    hit.Hit(damage, knockBack, attackPos);
                }
            }
        }

        public void SlashEffect()
        {
            GameObject go = Instantiate(slashEffect, transform.position, Quaternion.identity);
            if (go != null)
            {
                Animator animSlash = go.GetComponent<Animator>();
                if (animSlash != null)
                {
                    animSlash.Play("SlashAttack");
                }
            }
        }

        public void Hit(int amountDamage, float knockBackForce, Vector2 posAttacker)
        {
            if(actualHP > 0)
            {
                actualHP -= amountDamage;
                updateUIData?.Invoke(amountDamage);
                VFXManager.Get()?.ShakeScreen(.15f, .15f);
                SlashEffect();

                stunned = true;

                Vector2 directionKnockback = rb.position - posAttacker;
                directionKnockback.Normalize();
                rb.velocity = directionKnockback * knockBackForce;
            }

            if(actualHP <= 0)
            {
                actualHP = 0;
                Die();
            }
        }
        public void Die()
        {
            deathEvent?.Invoke();
            Destroy(gameObject, .15f);
        }
    }
}