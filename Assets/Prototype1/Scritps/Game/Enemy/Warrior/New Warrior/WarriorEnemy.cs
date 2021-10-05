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
        [SerializeField] public Rigidbody2D rb;
        [SerializeField] LayerMask playerLayer;
        [SerializeField] SpriteRenderer spriteEnemy;
        [SerializeField] GameObject slashEffect;
        [SerializeField] Animator enemyAnim;
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
        [SerializeField] public float distanceToTargeting;
        [SerializeField] public bool stunned;
        [SerializeField] public float timeStunned;
        [SerializeField] public float tStunn;

        [SerializeField] int impactsToStunn;
        int impacts;
        bool startKnockBack;
        [SerializeField] AnimationCurve knockBackCurve;
        [SerializeField] float tKnock;

        PlayerMovement player;

        private void Start()
        {
            startKnockBack = false;
            target = GameObject.FindGameObjectWithTag("Player");
            actualHP = maxHP;
            player = target.GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            if(startKnockBack)
            {
                tKnock += Time.fixedDeltaTime;

                if (knockBackCurve.Evaluate(tKnock) >= knockBackCurve.keys[knockBackCurve.length - 1].value)
                {
                    tKnock = 0;
                    startKnockBack = false;
                    rb.velocity = Vector2.zero;
                    enemyAnim.SetBool("hit", false);
                }
            }

            if (stunned)
            {
                tStunn += Time.fixedDeltaTime;

                if(tStunn >= timeStunned)
                {
                    tStunn = 0;
                    stunned = false;
                    rb.velocity = Vector2.zero;
                    impacts = 0;
                    enemyAnim.SetBool("stunned", stunned);
                }
            }

            FixSpriteSideAndSort();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0f), range);
        }

        void FixSpriteSideAndSort()
        {
            if (rb.position.x < target.transform.position.x)
                spriteEnemy.flipX = false;
            else
                spriteEnemy.flipX = true;

            if (player == null)
                return;

            if (rb.position.y < player.rig.position.y)
            {
                spriteEnemy.sortingOrder = 0;
                player.mySprite.sortingOrder = -1;
            }
            else
            {
                spriteEnemy.sortingOrder = -1;
                player.mySprite.sortingOrder = 0;
            }
        }

        public float GetDistanceToTarget(Vector2 position)
        {
            return Vector2.Distance(transform.position, position);
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
                
                impacts++;

                updateUIData?.Invoke(amountDamage);
                VFXManager.Get()?.ShakeScreen(.15f, .15f);
                SlashEffect();

                if(impacts >= impactsToStunn)
                {
                    stunned = true;
                    enemyAnim.SetBool("stunned", stunned);
                }
                enemyAnim.SetBool("hit", true);


                Vector2 directionKnockback = rb.position - posAttacker;
                directionKnockback.Normalize();
                rb.velocity = directionKnockback * knockBackForce;
                startKnockBack = true;
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