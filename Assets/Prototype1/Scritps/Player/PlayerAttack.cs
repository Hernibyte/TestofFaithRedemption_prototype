using UnityEngine;
using UnityEngine.SceneManagement;

namespace Proto1
{
    public class PlayerAttack : MonoBehaviour, IHittable
    {
        [SerializeField] LayerMask enemyLayer;
        [HideInInspector] public float horizontalAttack;
        [HideInInspector] public float verticalAttack;
        [SerializeField] public float distanceMelee;
        public float playerHP;
        public float maxPlayerHP;
        public float defensePlayer;
        public int playerDamage;
        public float playerKnockBackForce;
        [SerializeField] Animator playerAnimator;
        [SerializeField] Rigidbody2D rig;
        public float attackColdown;
        public float attackSpeed;

        [SerializeField] RangeAttack rangeMode;

        public delegate void UpdateUIData(int hitOnHP);
        public UpdateUIData updateUI;

        public delegate void PlayerHasAttack();
        public PlayerHasAttack attackFromPlayer;

        public PlayerMovement movementPlayer;

        void Start()
        {
            attackColdown = 0;
            maxPlayerHP = playerHP;
            movementPlayer = GetComponent<PlayerMovement>();
        }
    
        void Update()
        {
            Attack();
        }

        private void FixedUpdate()
        {
            GameManager gma = FindObjectOfType<GameManager>();
            if (gma.deck.updateDeck)
            {
                for (int i = 0; i < 7; i++)
                {
                    if(gma.GetCard(i).card != null)
                    {
                        maxPlayerHP += gma.GetCard(i).card.sCard.hp;
                        defensePlayer += gma.GetCard(i).card.sCard.defense;
                        playerDamage += gma.GetCard(i).card.sCard.damage;
                        playerKnockBackForce += gma.GetCard(i).card.sCard.knockback;
                        attackColdown += gma.GetCard(i).card.sCard.attackColdown;
                        attackSpeed += gma.GetCard(i).card.sCard.attackSpeed;
                        movementPlayer.speed += gma.GetCard(i).card.sCard.movementSpeed;
                    }
                }
                gma.deck.updateDeck = false;
            }
        }

        void MeleeAttack()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && attackColdown == 0)
            {
                attackColdown = attackSpeed;
                Vector2 attackPosition = new Vector2(transform.position.x + horizontalAttack, transform.position.y + verticalAttack);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPosition, distanceMelee, enemyLayer);
                foreach (Collider2D collider in colliders)
                {
                    IHittable hittable = collider.GetComponent<IHittable>();
                    if (hittable != null)
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

        void RangeAttack()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && attackColdown == 0)
            {
                attackFromPlayer?.Invoke();

                attackColdown = attackSpeed;

                rangeMode.Shoot();

                playerAnimator.SetTrigger("attack2");
            }

            if (attackColdown > 0)
                attackColdown -= Time.deltaTime;
            else
                attackColdown = 0;
        }

        void Attack()
        {
            MeleeAttack();

            RangeAttack();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(new Vector3(transform.position.x + horizontalAttack, transform.position.y + verticalAttack, 0f), distanceMelee);
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // worked
        }
    }
}
