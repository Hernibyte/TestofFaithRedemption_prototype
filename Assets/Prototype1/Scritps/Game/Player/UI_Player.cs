using UnityEngine;
using UnityEngine.UI;

namespace Proto1
{
    public class UI_Player : MonoBehaviour
    {
        [SerializeField] public PlayerAttack playerBasicStats;
        [SerializeField] public PlayerMovement playerMoveStats;
        [SerializeField] public Image healthBar;
        [SerializeField] public Image damageEntry;
        [SerializeField] public Text playerHPPorcent;
        [SerializeField] public Text playerStats;

        public bool needUpdate = false;
        float auxFillAmountDamage;
        int flagHealth;
        int amountHPGone;

        float amountHPFillImage;
        PlayerAttack.TypeUpdateUI typeUpdate;
        
        void Start()
        {
            flagHealth = 0;
            amountHPFillImage = 0;
            amountHPGone = 0;
            auxFillAmountDamage = healthBar.fillAmount;

            playerBasicStats.updateUI += HasRecivedDamage;
            playerBasicStats.updateUI += HasRecivedHealing;
        }

        void Update()
        {
            if(needUpdate)
            {
                float porcentHPPlayer = (playerBasicStats.actual_HP * 100) / playerBasicStats.max_HP;
                playerHPPorcent.text = porcentHPPlayer.ToString("00") + "%";

                switch (typeUpdate)
                {
                    case PlayerAttack.TypeUpdateUI.Damage:      UpdatePlayerHPDamage();
                        break;
                    case PlayerAttack.TypeUpdateUI.Healing:     UpdatePlayerHPHealing();
                        break;
                }
            }

            UpdatePlayerStats();
        }
        private void OnDisable()
        {
            playerBasicStats.updateUI -= HasRecivedDamage;
            playerBasicStats.updateUI -= HasRecivedHealing;
        }

        public void HasRecivedDamage(int amountDamageDealt, PlayerAttack.TypeUpdateUI type)
        {
            needUpdate = true;
            amountHPGone = amountDamageDealt;
            flagHealth = 1;
            typeUpdate = type;
        }

        public void HasRecivedHealing(int amountHeal, PlayerAttack.TypeUpdateUI type)
        {
            needUpdate = true;
            typeUpdate = type;
        }

        void UpdatePlayerHPHealing()
        {
            if(healthBar.fillAmount < playerBasicStats.actual_HP / playerBasicStats.max_HP)
            {
                healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, playerBasicStats.actual_HP / playerBasicStats.max_HP, Time.deltaTime);
                damageEntry.fillAmount = Mathf.MoveTowards(damageEntry.fillAmount, playerBasicStats.actual_HP / playerBasicStats.max_HP, Time.deltaTime);
            }

            if(healthBar.fillAmount >= playerBasicStats.actual_HP / playerBasicStats.max_HP)
            {
                healthBar.fillAmount = playerBasicStats.actual_HP / playerBasicStats.max_HP;
                damageEntry.fillAmount = playerBasicStats.actual_HP / playerBasicStats.max_HP;
                auxFillAmountDamage = damageEntry.fillAmount;
                needUpdate = false;
            }
        }

        void UpdatePlayerHPDamage()
        {
            amountHPFillImage = amountHPGone / playerBasicStats.max_HP;

            if (flagHealth == 1)
            {
                healthBar.fillAmount -= amountHPFillImage;
                flagHealth = 0;
            }

            if (damageEntry.fillAmount > (auxFillAmountDamage - amountHPFillImage))
                damageEntry.fillAmount -= Time.deltaTime;

            if (damageEntry.fillAmount <= (auxFillAmountDamage - amountHPFillImage))
            {
                damageEntry.fillAmount = (auxFillAmountDamage - amountHPFillImage);
                auxFillAmountDamage = damageEntry.fillAmount;
                needUpdate = false;
            }
        }

        void UpdatePlayerStats()
        {
            if (playerBasicStats == null || playerMoveStats == null || playerStats == null)
                return;

            playerStats.text = "-STATS-" + "\n" + "\n"
                + "HP: " + playerBasicStats.max_HP + "\n"
                + "DEF: " + playerBasicStats.defensePlayer + "\n"
                + "VEL: " + playerMoveStats.speed + "\n"
                + "A.SPD: "+ playerBasicStats.attackSpeedMelee + "\n"
                + "DPS: " + playerBasicStats.playerDamage + "\n";
        }
    }
}
