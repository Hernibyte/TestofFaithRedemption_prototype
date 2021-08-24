using UnityEngine;

namespace Proto1
{
    public class PlayerAttack : MonoBehaviour, IHittable
    {
        [SerializeField] LayerMask enemyLayer;
        [HideInInspector] public float horizontalAttack;
        [HideInInspector] public float verticalAttack;
        [SerializeField] public float rangeAttack;
        public float playerHP;
        public float maxPlayerHP;
        public float defensePlayer;
        public int playerDamage;
        public float playerKnockBackForce;
        [SerializeField] Animator playerAnimator;
        [SerializeField] Rigidbody2D rig;
        public float attackColdown;
        public float attackSpeed;

        public delegate void UpdateUIData(int hitOnHP);
        public UpdateUIData updateUI;

        void Start()
        {
            attackColdown = 0;
            maxPlayerHP = playerHP;
        }
    
        void Update()
        {
            Attack();
        }

        void Attack()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && attackColdown == 0)
            {
                attackColdown = attackSpeed;
                Vector2 attackPosition= new Vector2(transform.position.x + horizontalAttack, transform.position.y + verticalAttack);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPosition, rangeAttack, enemyLayer);
                foreach (Collider2D collider in colliders)
                {
                    IHittable hittable = collider.GetComponent<IHittable>();
                    if(hittable != null)
                    {
                        hittable.Hit(playerDamage, playerKnockBackForce, transform.position);
                    }
                }
                playerAnimator.SetTrigger("attack");
            }

            if (attackColdown > 0)
                attackColdown -= Time.deltaTime;
            else
                attackColdown = 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(new Vector3(transform.position.x + horizontalAttack, transform.position.y + verticalAttack, 0f), rangeAttack);
        }
        public void Hit(int damageAmount, float knockBackForce, Vector2 posAttacker)
        {
            if(playerHP > 0)
            {
                playerHP -= damageAmount;
                playerAnimator.SetTrigger("hit");
                if(transform.position.x > posAttacker.x)
                    rig.AddForce(Vector2.right * knockBackForce, ForceMode2D.Impulse);
                else
                    rig.AddForce(-Vector2.right * knockBackForce, ForceMode2D.Impulse);
                
                updateUI?.Invoke(damageAmount);
            }
            else
            {
                playerHP = 0;
            }

            if(playerHP <= 0)
            {
                playerHP = 0;
                Die();
            }
        }
        public void Die()
        {
            Destroy(gameObject);
        }
    }
}