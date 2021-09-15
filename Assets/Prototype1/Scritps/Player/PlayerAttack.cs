using UnityEngine;
using UnityEngine.SceneManagement;

namespace Proto1
{
    public class PlayerAttack : MonoBehaviour, IHittable
    {
        [Header("PLAYER NEEDS")]
        [SerializeField] LayerMask enemyLayer;
        [SerializeField] Transform meleeAttackPoint;
        [SerializeField] GameObject slashEffect;
        [SerializeField] Animator playerAnimator;
        [SerializeField] Rigidbody2D rig;
        [SerializeField] RangeAttack rangeMode;
        public PlayerMovement movementPlayer;

        [Header("NORMAL STATS")]
        [Space(15)]
        public float max_HP;
        public float actual_HP;
        public float defensePlayer;
        [Range(5,95)]public float defenseEfficiency;
        [Range(15,98)]public float DMGReducedByDEF;

        [Header("ATTACK STATS")]
        public int playerDamage;
        public float distanceMelee;
        public float knockBackMelee;
        public float attackColdownMelee;
        public float attackSpeedMelee;
        public float attackColdownRanged;
        public float attackSpeedRanged;

        public delegate void UpdateUIData(int hitOnHP);
        public UpdateUIData updateUI;

        public delegate void PlayerHasAttack();
        public PlayerHasAttack attackFromPlayer;

        void Start()
        {
            attackColdownMelee = 0;
            actual_HP = max_HP;
            movementPlayer = GetComponent<PlayerMovement>();

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
        void Update()
        {
            if (attackColdownMelee > 0)
                attackColdownMelee -= Time.deltaTime;
            else
                attackColdownMelee = 0;

            if (attackColdownRanged > 0)
                attackColdownRanged -= Time.deltaTime;
            else
                attackColdownRanged = 0;

            if (Input.GetKeyDown(KeyCode.Mouse0) && attackColdownMelee == 0)
            {
                attackColdownMelee = attackSpeedMelee;
                playerAnimator.SetTrigger("attack");
            }

            RangeAttack();
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
                        max_HP += gma.GetCard(i).card.sCard.hp;
                        defensePlayer += gma.GetCard(i).card.sCard.defense;
                        playerDamage += gma.GetCard(i).card.sCard.damage;
                        knockBackMelee += gma.GetCard(i).card.sCard.knockback;
                        attackColdownMelee += gma.GetCard(i).card.sCard.attackColdown;
                        attackSpeedMelee += gma.GetCard(i).card.sCard.attackSpeed;
                        movementPlayer.speed += gma.GetCard(i).card.sCard.movementSpeed;
                    }
                }
                gma.deck.updateDeck = false;
            }
        }

        public void MeleeAttack()
        {
            Vector2 attackPosition = new Vector2(meleeAttackPoint.position.x, meleeAttackPoint.position.y);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPosition, distanceMelee, enemyLayer);
            foreach (Collider2D collider in colliders)
            {
                IHittable hittable = collider.GetComponent<IHittable>();
                if (hittable != null)
                {
                    hittable.Hit(playerDamage, knockBackMelee, transform.position);

                    GameObject go = Instantiate(slashEffect, meleeAttackPoint.transform.position, Quaternion.identity);
                    if(go != null)
                    {
                        Animator animSlash = go.GetComponent<Animator>();
                        if(animSlash != null)
                        {
                            animSlash.Play("SlashAttack");
                        }
                    }

                    VFXManager.Get()?.ShakeScreen(.15f, .16f);
                }
            }
        }

        public void RangeAttack()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && attackColdownRanged == 0)
            {
                attackFromPlayer?.Invoke();

                attackColdownRanged = attackSpeedRanged;

                rangeMode.Shoot();            
                
                playerAnimator.SetTrigger("attack2");
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(new Vector3(meleeAttackPoint.position.x, meleeAttackPoint.position.y, 0f), distanceMelee);
        }
        public void Hit(int damageAmount, float knockBackForce, Vector2 posAttacker)
        {
            //Eficiencia de la defensa con respecto a daño plano y defensa porcentual

            float rawDefense = defensePlayer;
            float defensePorcentage = ( rawDefense * defenseEfficiency) / 100f; //Defensa porcentual con respecto a eficiencia PORCENTUAL
            float finalDefense = (defensePorcentage * DMGReducedByDEF) / 100f; //Defensa final con respecto a daño reducido PORCENTUAL
            float damageReduce = (damageAmount * finalDefense) / 100f; //Daño reducido final como porcentaje del 100% del daño plano

            float damageEntry = damageAmount - damageReduce;  //Daño reducido

            Debug.Log("Damage Recived:" + damageEntry);

            if(actual_HP > 0)
            {
                actual_HP -= (int)damageEntry;

                playerAnimator.SetTrigger("hit"); 

                if(transform.position.x > posAttacker.x)
                    rig.AddForce(Vector2.right * knockBackForce, ForceMode2D.Impulse);
                else
                    rig.AddForce(-Vector2.right * knockBackForce, ForceMode2D.Impulse);

                VFXManager.Get()?.ShakeScreen(.15f, .15f);

                updateUI?.Invoke((int)damageEntry);
            }
            else
            {
                actual_HP = 0;
            }

            if(actual_HP <= 0)
            {
                actual_HP = 0;
                Die();
            }
        }
        public void Die()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // worked
        }
    }
}
