using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField] Animator bossAnimator;

        [Header("BOSS STATS")]
        [Space(20)]
        [SerializeField] public float bossSpeed;
        [SerializeField] public float attackRange;
        [SerializeField] public int bossDamage;
        [SerializeField] public float bossKnockBack;
        [SerializeField] public float bossActualHP;
        [SerializeField] public float bossMAX_HP;
        [SerializeField] public bool isInvulnerable;
        [SerializeField] public bool isEnraged;

        [Header("NO TOCAR")]
        [Space(20)]
        [SerializeField] float colliderOffsetXRight;
        [SerializeField] float colliderOffsetXLeft;

        BoxCollider2D bossCollider;

        public delegate void UpdateBossUIData(int amountDamage);
        public UpdateBossUIData updateUIData;

        CameraShake camShake;

        void Start()
        {
            bossCollider = gameObject.GetComponent<BoxCollider2D>();
            camShake = Camera.main.GetComponent<CameraShake>();

            bossActualHP = bossMAX_HP;
            isEnraged = false;

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

        public void EnragedAttack()
        {
            Vector2 attackPosition = new Vector2(rb.position.x, rb.position.y);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPosition, attackRange, playerLayer);
            foreach (Collider2D collider in colliders)
            {
                IHittable hittable = collider.GetComponent<IHittable>();
                if (hittable != null)
                {
                    hittable.Hit(bossDamage * 2, bossKnockBack * 1.5f, transform.position);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0f), attackRange);
        }
        public void Hit(int damage, float kncokBack, Vector2 posAttacker)
        {
            if (isInvulnerable)
                return;

            if (bossActualHP > 0)
            {
                bossActualHP -= damage;

                bossAnimator.SetTrigger("Hit");

                if (camShake != null)
                    StartCoroutine(camShake.Shake(.15f, .2f));

                updateUIData?.Invoke(damage);
            }

            if(bossActualHP <= bossMAX_HP / 2f && !isEnraged)
            {
                bossAnimator.SetBool("IsEnraged", true);
                bossSpeed += 1.5f;
                isEnraged = true;
            }

            if (bossActualHP <= 0)
            {
                bossActualHP = 0;
                bossAnimator.SetBool("IsDead",true);
                Die();
            }
        }
        public void Die()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // worked
        }
    }
}