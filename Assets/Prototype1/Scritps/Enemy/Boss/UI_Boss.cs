using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Proto1
{
    public class UI_Boss : MonoBehaviour
    {
        public bool bossIsActive;
        [SerializeField] Image healtbarBoss;
        [SerializeField] Image damageEntryBoss;
        [SerializeField] GameObject uiHealthBarBoss;
        [SerializeField] BossEnemy boss;

        float amountHPLose;
        float auxFillAmount;
        float amountHPFillImage;
        int flagHealth;
        public bool enemyUIUpdate;

        int triggerBoss;

        private void OnEnable()
        {
            if (FindObjectOfType<BossEnemy>())
            {
                boss = FindObjectOfType<BossEnemy>();

                bossIsActive = false;

                if (!bossIsActive)
                {
                    boss.gameObject.SetActive(false);
                    uiHealthBarBoss.gameObject.SetActive(false);
                    triggerBoss = 0;
                }

                flagHealth = 0;
                amountHPFillImage = 0;
                auxFillAmount = damageEntryBoss.fillAmount;
                amountHPLose = 0;

                boss.updateUIData += AskForUpdate;
            }
        }
    
        void Update()
        {
            if (!bossIsActive)
            {
                triggerBoss = 0;
                return;
            }
    
            if(triggerBoss == 0)
            {
                boss.gameObject.SetActive(true);
                uiHealthBarBoss.gameObject.SetActive(true);
                triggerBoss = 1;
            }

            if (enemyUIUpdate)
            {
                UpdateDataBoss();
            }
        }

        void AskForUpdate(int amountDamage)
        {
            enemyUIUpdate = true;
            amountHPLose = amountDamage;
            flagHealth = 1;
        }
        void UpdateDataBoss()
        {
            amountHPFillImage = (amountHPLose * 1) / boss.bossMAX_HP;

            if(flagHealth == 1)
            {
                healtbarBoss.fillAmount -= amountHPFillImage;
                flagHealth = 0;
            }

            if(damageEntryBoss.fillAmount > (auxFillAmount - amountHPFillImage))
                damageEntryBoss.fillAmount -= Time.deltaTime;

            if (damageEntryBoss.fillAmount <= (auxFillAmount - amountHPFillImage))
            {
                damageEntryBoss.fillAmount = (auxFillAmount - amountHPFillImage);
                auxFillAmount = damageEntryBoss.fillAmount;
                enemyUIUpdate = false;
            }
        }
    }
}