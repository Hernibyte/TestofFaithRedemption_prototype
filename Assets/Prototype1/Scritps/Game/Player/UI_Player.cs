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
        [SerializeField] public Text playerStats;

        public bool needUpdateData = false;
        float auxFillAmount;
        int flagHealth;
        int amountHPGone;
        float amountHPFillImage;
        
        void Start()
        {
            flagHealth = 0;
            amountHPFillImage = 0;
            amountHPGone = 0;
            auxFillAmount = healthBar.fillAmount;

            playerBasicStats.updateUI += AskForUpdate;
        }

        void Update()
        {
            if(needUpdateData)
            {
                UpdatePlayerHP();
            }

            UpdatePlayerStats();
        }
        private void OnDisable()
        {
            playerBasicStats.updateUI -= AskForUpdate;
        }

        public void AskForUpdate(int amountDamageDealt)
        {
            needUpdateData = true;
            amountHPGone = amountDamageDealt;
            flagHealth = 1;
        }

        void UpdatePlayerHP()
        {
            amountHPFillImage = (amountHPGone * 1) / playerBasicStats.max_HP;

            if(flagHealth == 1)
            {
                healthBar.fillAmount -= amountHPFillImage;
                flagHealth = 0;
            }

            if (damageEntry.fillAmount > (auxFillAmount - amountHPFillImage))
                damageEntry.fillAmount -= Time.deltaTime;

            if (damageEntry.fillAmount <= (auxFillAmount - amountHPFillImage))
            {
                damageEntry.fillAmount = (auxFillAmount - amountHPFillImage);
                auxFillAmount = damageEntry.fillAmount;
                needUpdateData = false;
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
