using UnityEngine;
using UnityEngine.UI;

namespace Proto1
{
    public class UI_Player : MonoBehaviour
    {
        [SerializeField] public PlayerAttack player;
        [SerializeField] public Image healthBar;

        public bool needUpdateData = false;
        float delayToReduceHP;
        float timer;
        float auxFillAmount;
        int amountHPGone;
        
        void Start()
        {
            delayToReduceHP = 1.5f;
            timer = 0;
            amountHPGone = 0;
            auxFillAmount = healthBar.fillAmount;

            player.updateUI += AskForUpdate;
        }

        void Update()
        {
            if(needUpdateData)
            {
                if(timer < delayToReduceHP)
                {
                    timer += Time.deltaTime;

                    UpdatePlayerHP();
                }
                else
                {
                    timer = 0;
                    auxFillAmount = healthBar.fillAmount;
                    needUpdateData = false;
                }
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
        }

        void UpdatePlayerHP()
        {
            float amountToRest = (amountHPGone * 1) / player.maxPlayerHP;

            if (healthBar.fillAmount >= (auxFillAmount - amountToRest))
                healthBar.fillAmount -= Time.deltaTime;
        }
    }
}
