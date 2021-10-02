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
        [SerializeField] StatsMenu stats;
        [SerializeField] CardTaked cardTakenSystem;
        public PlayerMovement movementPlayer;
        [SerializeField] Texture2D cursorTex;

        [Header("NORMAL STATS")]
        [Space(15)]
        public float max_HP;
        public float actual_HP;
        public float playerCapHP;
        public float playerMinHP;
        [Header("--------------")]
        public float defensePlayer;
        [Range(5,95)]public float defenseEfficiency;
        [Range(15,98)]public float DMGReducedByDEF;
        public float maxDefenseStack;
        public float minDefenseStack;

        [Header("ATTACK STATS")]
        public int playerDamage;
        [Range(5, 15)] public int minPlayerDamage;
        public float distanceMelee;
        public float knockBackMelee;
        public float attackColdownMelee;
        public float attackSpeedMelee;
        public float attackColdownRanged;
        public float attackSpeedRanged;
        public float meleeImpulseForce1;

        public delegate void UpdateUIData(int hitOnHP);
        public UpdateUIData updateUI;

        public delegate void PlayerHasAttack();
        public PlayerHasAttack attackFromPlayer;

        public GameManager gmRef;

        void Start()
        {
            attackColdownMelee = 0;
            actual_HP = max_HP;
            movementPlayer = GetComponent<PlayerMovement>();

            gmRef = FindObjectOfType<GameManager>();

            if(gmRef != null)
            {
                gmRef.deck.updateSlotOfCard += UpdateSlotOfCard;
            }

            Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.Auto);
        }
        void Update()
        {
            if (!movementPlayer.activateGameplay)
                return;

            if (attackColdownMelee > 0)
                attackColdownMelee -= Time.deltaTime;
            else
                attackColdownMelee = 0;

            if (attackColdownRanged > 0)
                attackColdownRanged -= Time.deltaTime;
            else
                attackColdownRanged = 0;

            if(!stats.openStats)
            {
                if(!cardTakenSystem.isOpen)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0) && attackColdownMelee == 0)
                    {
                        attackColdownMelee = attackSpeedMelee;
                        playerAnimator.SetTrigger("attack");
                    }
                    //
                    RangeAttack();
                }
            }
        }

        private void OnDisable()
        {
            if (gmRef != null)
            {
                gmRef.deck.updateSlotOfCard -= UpdateSlotOfCard;
            }
        }

        public void UpdateSlotOfCard(int indexSlot)
        {
            if (gmRef == null)
            {
                Debug.LogWarning("No se ha encontrado la referencia del Game Manager en PlayerAttack[Script]");
                return;
            }

            if(gmRef.GetCard(indexSlot).card != null)
            {
                NewSCard cardTaked = gmRef.GetCard(indexSlot).card.sCard;

                //BASIC STATS
                CalcBasicStats(cardTaked);

                //OTHER STATS
                CalcKnockback(cardTaked);
                CalcAttackSpeed(cardTaked);

                gmRef.deck.updateDeck = false;
            }
        }
        public void CalcBasicStats(NewSCard cardTaked)
        {
            for (int i = 0; i < cardTaked.typeStats.Count; i++)
            {
                switch (cardTaked.typeStats[i])
                {
                    case NewSCard.CardType.HP_Plane:

                        if (cardTaked.hp > 0)
                        {
                            //Positivo
                            if (max_HP < playerCapHP)
                                max_HP += cardTaked.hp;
                            else
                                max_HP = playerCapHP;
                        }
                        else
                        {
                            //Negativo
                            if (max_HP > cardTaked.hp)
                                max_HP -= cardTaked.hp;
                            else
                                max_HP = 1;
                        }

                        if (max_HP < 0)
                            max_HP = playerMinHP;
                        else if (max_HP > playerCapHP)
                            max_HP = playerCapHP;

                        break;
                    case NewSCard.CardType.HP_Porcent:

                        if (cardTaked.hp > 0)
                        {
                            //Positivo
                            if (max_HP < playerCapHP)
                            {
                                float amountHpPorcent = cardTaked.hp;
                                float totalHp = max_HP;
                                float finalHPIncreased = (totalHp * amountHpPorcent) / 100;

                                max_HP += finalHPIncreased;
                            }
                            else
                                max_HP = playerCapHP;
                        }
                        else
                        {
                            //Negativo
                            float amountHpPorcent = Mathf.Abs(cardTaked.hp);
                            float totalHp = max_HP;
                            float finalHPDecreased = (totalHp * amountHpPorcent) / 100;

                            if (max_HP > finalHPDecreased)
                                max_HP -= finalHPDecreased;
                        }

                        if (max_HP < 0)
                            max_HP = playerMinHP;
                        else if (max_HP > playerCapHP)
                            max_HP = playerCapHP;

                        break;
                    case NewSCard.CardType.ATK_Plane:

                        if (cardTaked.damage > 0)
                            playerDamage += cardTaked.damage;
                        else
                        {
                            if (playerDamage > cardTaked.damage)
                                playerDamage -= cardTaked.damage;
                        }

                        if (playerDamage < 0)
                            playerDamage = minPlayerDamage;


                        break;
                    case NewSCard.CardType.ATK_Porcent:

                        if (cardTaked.damage > 0)
                        {
                            //Positivo
                            float amountDamagePorcent = cardTaked.damage;
                            float actualDamage = playerDamage;
                            float finalDamageIncrease = (actualDamage * amountDamagePorcent) / 100;

                            playerDamage += (int)finalDamageIncrease;
                        }
                        else
                        {
                            //Negativo
                            float amountDamagePorcent = Mathf.Abs(cardTaked.damage);
                            float actualDamage = playerDamage;
                            float finalDamageDecrease = (actualDamage * amountDamagePorcent) / 100;

                            if (playerDamage > finalDamageDecrease)
                                playerDamage -= (int)finalDamageDecrease;
                        }

                        if (playerDamage < 0)
                            playerDamage = minPlayerDamage;

                        break;
                    case NewSCard.CardType.DEF_Plane:

                        if (cardTaked.defense > 0)
                            defensePlayer += cardTaked.defense;
                        else
                        {
                            if (defensePlayer > cardTaked.defense)
                                defensePlayer -= cardTaked.defense;
                        }

                        if (defensePlayer < 0)
                            defensePlayer = minDefenseStack;
                        else if (defensePlayer > maxDefenseStack)
                            defensePlayer = maxDefenseStack;

                        break;
                    case NewSCard.CardType.DEF_Porcent:

                        if (cardTaked.defense > 0)
                        {
                            //Positivo
                            float amountDefPorcent = cardTaked.defense;
                            float defenseRaw = defensePlayer;
                            float amountIncrease = (defenseRaw * amountDefPorcent) / 100;

                            defensePlayer = amountIncrease;
                        }
                        else
                        {
                            //Negativo
                            float amountDefPorcent = Mathf.Abs(cardTaked.defense);
                            float defenseRaw = defensePlayer;
                            float amountDecrease = (defenseRaw * amountDefPorcent) / 100;

                            if (defensePlayer > amountDecrease)
                                defensePlayer -= amountDecrease;
                        }

                        if (defensePlayer < 0)
                            defensePlayer = minDefenseStack;
                        else if (defensePlayer > maxDefenseStack)
                            defensePlayer = maxDefenseStack;

                        break;
                    case NewSCard.CardType.SPD_Plane:

                        if (cardTaked.movementSpeed > 0)
                        {
                            //Positivo
                            if (movementPlayer.speed < movementPlayer.playerSpeedCap)
                                movementPlayer.speed += cardTaked.movementSpeed;
                        }
                        else
                        {
                            //Negativo
                            if (movementPlayer.speed > cardTaked.movementSpeed)
                                movementPlayer.speed -= cardTaked.movementSpeed;
                        }

                        if (movementPlayer.speed < 0)
                            movementPlayer.speed = movementPlayer.playerMinSpeed;
                        else if (movementPlayer.speed > movementPlayer.playerSpeedCap)
                            movementPlayer.speed = movementPlayer.playerSpeedCap;

                        break;
                    default:
                        break;
                }
            }
        }

        public void CalcKnockback(NewSCard cardTaked)
        {
            if (cardTaked.knockback > 0)
                knockBackMelee += cardTaked.knockback;
            else
            {
                if (knockBackMelee > cardTaked.knockback)
                    knockBackMelee -= cardTaked.knockback;
                else
                    knockBackMelee = 1;
            }
        }

        public void CalcAttackSpeed(NewSCard cardTaked)
        {
            if (cardTaked.attackSpeed > 0)
            {
                attackSpeedMelee += cardTaked.attackSpeed;
                attackSpeedRanged += cardTaked.attackSpeed;
            }
            else
            {
                if (attackSpeedRanged > cardTaked.attackSpeed)
                    attackSpeedRanged -= cardTaked.attackSpeed;
                else
                    attackSpeedRanged = 0.1f;

                if (attackSpeedMelee > cardTaked.attackSpeed)
                    attackSpeedMelee -= cardTaked.attackSpeed;
                else
                    attackSpeedMelee = 0.1f;
            }
        }

        public void MeleeAttack()
        {
            Vector2 directionImpulse = transform.position - meleeAttackPoint.position;
            directionImpulse.Normalize();
            rig.AddForce(-directionImpulse * meleeImpulseForce1, ForceMode2D.Impulse);

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
            VFXManager.Get()?.StopShaking();
            UI_PlayerDeath myEvent = FindObjectOfType<UI_PlayerDeath>();
            myEvent.Activate();
        }
    }
}
