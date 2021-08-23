using UnityEngine;

namespace Proto1
{
    public class Enemy : MonoBehaviour, IHittable
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

        [SerializeField] float enemyHP;
        [SerializeField] int enemyDamage;
        [SerializeField] float enemyKnockBackForce;
        [SerializeField] float rangeAttack;

        private float timeToRestore;
        private float coldownAfterHit;

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
            enemyState = STATENEMY.Idle;
            target = GameObject.FindGameObjectWithTag("Player");
        }
    
        void Update()
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

            //Debug.Log("Distancia:" + Vector2.Distance(transform.position, target.transform.position).ToString());

            if(Vector2.Distance(transform.position, target.transform.position) < distanceToTarget &&
               Vector2.Distance(transform.position, target.transform.position) > maxNearDistance)
            {
                enemyAnimator.SetInteger("following", (int)enemyState);

                rig.MovePosition(Vector2.Lerp(transform.position, target.transform.position, (enemySpeed / 3) * Time.deltaTime));

                if (transform.position.x > target.transform.position.x)
                    spriteEnemy.flipX = true;
                else
                    spriteEnemy.flipX = false;
            }
            else if(Vector2.Distance(transform.position, target.transform.position) <= maxNearDistance)
            {
                enemyState = STATENEMY.Attacking;
                //rig.velocity = Vector2.zero;
            }
        }

        void Attack()
        {
            enemyAnimator.SetFloat("prepareAttack", attackDelay);

            if (attackDelay > 0)
            {
                if (attackDelay > 0)
                    attackDelay -= Time.deltaTime;
                else
                {
                    attackDelay = 0;
                }
                return;
            }

            if (attackColdown == 0)
            {
                attackColdown = 1.0f;
                enemyAnimator.SetTrigger("attack");

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
            
            if (attackColdown > 0)
                attackColdown -= Time.deltaTime;
            else
            {
                attackColdown = 0;
                attackDelay = maxAttackDelay;

                if (target != null)
                    enemyState = STATENEMY.Following;
                else
                    enemyState = STATENEMY.Idle;
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
                attackColdown = 1;
                enemyState = STATENEMY.BeignDamaged;

                //directionWhereDamage = posAttacker - new Vector2(transform.position.x, transform.position.y);

                if (transform.position.x > posAttacker.x)
                    rig.velocity = new Vector2(knockBackForce,0);
                else
                    rig.velocity = new Vector2(-knockBackForce,0);
            }
            else
            {
                enemyHP = 0;
                Die();
            }
        }
        public void Die()
        {
            Destroy(gameObject);
        }
    }
}
