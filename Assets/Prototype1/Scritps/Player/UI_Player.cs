using UnityEngine;
using UnityEngine.UI;

namespace Proto1
{
    public class UI_Player : MonoBehaviour
    {
        [SerializeField] public PlayerAttack player;
        [SerializeField] public Image healthBar;
        [SerializeField] public Image damageEntry;

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

            player.updateUI += AskForUpdate;
        }

        void Update()
        {
            if(needUpdateData)
            {
                UpdatePlayerHP();
            }
        }
        private void OnDisable()
        {
            player.updateUI -= AskForUpdate;
        }

        public void AskForUpdate(int amountDamageDealt)
        {
            needUpdateData = true;
            amountHPGone = amountDamageDealt;
            flagHealth = 1;
        }

        void UpdatePlayerHP()
        {
            amountHPFillImage = (amountHPGone * 1) / player.maxPlayerHP;

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
    }
}
