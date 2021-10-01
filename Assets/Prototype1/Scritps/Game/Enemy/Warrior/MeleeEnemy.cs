using UnityEngine;
using UnityEngine.Events;

namespace Proto1
{
    public class MeleeEnemy : MonoBehaviour, IHittable
    {
        [SerializeField] LayerMask playerLayer;
        [SerializeField] Animator enemyAnimator;
        [SerializeField] SpriteRenderer spriteEnemy;
        [SerializeField] Rigidbody2D rig;
        [SerializeField] float enemySpeed;
        public float attackColdown;
        public float attackDelay;
        public float maxAttackDelay;

        [SerializeField] GameObject target;
        public float distanceToTarget;
        public float maxNearDistance;

        [SerializeField] public float enemyHP;
        [SerializeField] public float enemyMaxHP;
        [SerializeField] int enemyDamage;
        [SerializeField] float enemyKnockBackForce;
        [SerializeField] float rangeAttack;

        private float timeToRestore;
        private float coldownAfterHit;

        public delegate void UpdateEnemyUIData(int amountDamage);
        public UpdateEnemyUIData updateUIData;

        public UnityEvent deathEvent;

        [System.Serializable]
        public enum STATENEMY
        {
            Idle,
            Attacking,
            Following,
            BeignDamaged
        }
        public STATENEMY enemyState;

        void Start()
        {
            timeToRestore = 0;
            coldownAfterHit = 0.1f;
            enemyMaxHP = enemyHP;
            enemyState = STATENEMY.Idle;

            attackDelay = maxAttackDelay;
            attackColdown = 0;

            target = GameObject.FindGameObjectWithTag("Player");

            GameManager gma = FindObjectOfType<GameManager>();
            for (int i = 0; i < 7; i++)
            {
                if(gma.GetCard(i).card != null) {
                    enemyMaxHP += gma.GetCard(i).card.sCard.hp;
                    //enemySpeed += gma.GetCard(i).card.sCard.movementSpeed;
                    attackColdown += gma.GetCard(i).card.sCard.attackColdown;
                    maxAttackDelay += gma.GetCard(i).card.sCard.attackDelay;
                    enemyDamage += gma.GetCard(i).card.sCard.damage;
                    enemyKnockBackForce += gma.GetCard(i).card.sCard.knockback;
                }
            }

            enemyHP = enemyMaxHP;
        }
    
        void FixedUpdate()
        {
            switch (enemyState)
            {
                case STATENEMY.Idle:

                    enemyAnimator.SetInteger("following", (int)enemyState);
                    if(target != null)
                    {
                        if (Vector2.Distance(transform.position, target.transform.position) < distanceToTarget &&
                            Vector2.Distance(transform.position, target.transform.position) > maxNearDistance)
                        {
                            enemyState = STATENEMY.Following;
                        }
                    }

                    break;
                case STATENEMY.Attacking:

                    Attack();

                    break;
                case STATENEMY.Following:

                    GoToTarget();

                    break;

                case STATENEMY.BeignDamaged:

                    enemyAnimator.SetTrigger("hit");

                    if (timeToRestore < coldownAfterHit)
                        timeToRestore += Time.deltaTime;
                    else
                    {
                        timeToRestore = 0;
                        rig.velocity = Vector2.zero;
                        enemyState = STATENEMY.Following;
                    }
                    break;
            }
        }

        void GoToTarget()
        {
            if (target == null)
            {
                enemyAnimator.SetInteger("following", (int)enemyState);
                return;
            }

            if (Vector2.Distance(transform.position, target.transform.position) > distanceToTarget)
            {
                enemyState = STATENEMY.Idle;
            }

            if (Vector2.Distance(transform.position, target.transform.position) < distanceToTarget &&
               Vector2.Distance(transform.position, target.transform.position) > maxNearDistance)
            {
                enemyAnimator.SetInteger("following", (int)enemyState);

                rig.MovePosition(Vector2.MoveTowards(transform.position, target.transform.position, (enemySpeed) * Time.deltaTime));

                if (transform.position.x > target.transform.position.x)
                    spriteEnemy.flipX = true;
                else
                    spriteEnemy.flipX = false;
            }
            else if(Vector2.Distance(transform.position, target.transform.position) <= maxNearDistance)
            {
                enemyState = STATENEMY.Attacking;
            }
        }

        public void MeleeAttack()
        {
            Vector2 attackPosition = new Vector2(transform.position.x, transform.position.y);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPosition, rangeAttack, playerLayer);
            foreach (Collider2D collider in colliders)
            {
                IHittable hittable = collider.GetComponent<IHittable>();
                if (hittable != null)
                {
                    hittable.Hit(enemyDamage, enemyKnockBackForce, transform.position);
                }
            }
        }

        void Attack()
        {
            //PREPARACION DEL ATAQUE

            enemyAnimator.SetFloat("prepareAttack", attackDelay);

            if (attackDelay > 0)
            {
                if (attackDelay > 0)
                    attackDelay -= Time.deltaTime;
                else
                    attackDelay = 0;
                
                return;
            }
            else
            {
                 enemyAnimator.SetTrigger("attack");
            }

            if (attackColdown == 0)
            {
                attackColdown = 1.0f;
            }
            
            //COLDOWN PER HIT (Esto calcula el tiempo entre ataque del enemy)

            if (attackColdown > 0)
                attackColdown -= Time.deltaTime;
            else
            {
                attackColdown = 0;
                attackDelay = maxAttackDelay;
                if (target != null)
                {
                    if(Vector2.Distance(transform.position, target.transform.position) > distanceToTarget)
                        enemyState = STATENEMY.Idle;
                    else
                        enemyState = STATENEMY.Following;
                }
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0f), rangeAttack);
        }
        public void Hit(int amountDamage, float knockBackForce, Vector2 posAttacker)
        {
            if(enemyHP > 0)
            {
                enemyHP -= amountDamage;
                attackDelay = 0;

                enemyAnimator.SetFloat("prepareAttack", attackDelay);
                enemyAnimator.SetFloat("Knockback", knockBackForce);
                attackColdown = 1;

                enemyState = STATENEMY.BeignDamaged;

                VFXManager.Get()?.ShakeScreen(.15f,.15f);

                if (transform.position.x > posAttacker.x)
                    rig.velocity = new Vector2(knockBackForce,0);
                else
                    rig.velocity = new Vector2(-knockBackForce,0);

                updateUIData?.Invoke(amountDamage);
            }
            else
            {
                enemyHP = 0;
            }

            if(enemyHP <= 0)
            {
                enemyHP = 0;
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
