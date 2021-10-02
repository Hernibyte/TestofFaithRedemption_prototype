using UnityEngine;
using UnityEngine.UI;

namespace Proto1
{
    public class UI_Enemy : MonoBehaviour
    {
        [SerializeField]
        public enum TYPENEMY
        {
            Ranger,
            Warrior
        }
        public TYPENEMY enemyType;
        [SerializeField] MeleeEnemy enemyReference;// Obsolet xd
        [SerializeField] RangeEnemy enemyReference2;
        [SerializeField] WarriorEnemy enemyReferenc3;
        [SerializeField] Image enemyHealthBar; 
        [SerializeField] Image enemyDamageEntry;
        float amountHPLose;
        float auxFillAmount;
        float amountHPFillImage;
        int flagHealth;
        public bool enemyUIUpdate;
        void Start()
        {
            flagHealth = 0;
            amountHPFillImage = 0;
            auxFillAmount = enemyDamageEntry.fillAmount;
            amountHPLose = 0;

            if(enemyReference != null)
                enemyReference.updateUIData += AskForUpdate;

            if(enemyReference2 != null)
                enemyReference2.updateUIData += AskForUpdate;
            
            if(enemyReferenc3 != null)
                enemyReferenc3.updateUIData += AskForUpdate;
        }
    
        void Update()
        {
            if(enemyUIUpdate)
            {
                switch (enemyType)
                {
                    case TYPENEMY.Ranger:  UpdateEnemyRangerHP();
                        break;
                    case TYPENEMY.Warrior:
                        UpdateEnemy_OLD_WarriorHP();
                        UpdateEnemy_NEW_WarriorHP();
                        break;
                }
            }
        }

        private void OnDisable()
        {
            if(enemyReference != null)
                enemyReference.updateUIData -= AskForUpdate;

            if(enemyReference2 != null)
                enemyReference2.updateUIData -= AskForUpdate;
            
            if(enemyReferenc3 != null)
                enemyReferenc3.updateUIData -= AskForUpdate;
        }

        void AskForUpdate(int amountDamage)
        {
            enemyUIUpdate = true;
            amountHPLose = amountDamage;
            flagHealth = 1;
        }

        void UpdateEnemy_OLD_WarriorHP()
        {
            if (enemyReference == null)
                return;

            amountHPFillImage = (amountHPLose * 1) / enemyReference.enemyMaxHP;

            if(flagHealth == 1)
            {
                enemyHealthBar.fillAmount -= amountHPFillImage;
                flagHealth = 0;
            }

            if (enemyDamageEntry.fillAmount > (auxFillAmount - amountHPFillImage))
                enemyDamageEntry.fillAmount -= Time.deltaTime;

            if (enemyDamageEntry.fillAmount <= (auxFillAmount - amountHPFillImage))
            {
                enemyDamageEntry.fillAmount = (auxFillAmount - amountHPFillImage);
                auxFillAmount = enemyDamageEntry.fillAmount;
                enemyUIUpdate = false;
            }
        }

        void UpdateEnemy_NEW_WarriorHP()
        {
            if (enemyReferenc3 == null)
                return;

            amountHPFillImage = (amountHPLose * 1) / enemyReferenc3.maxHP;

            if (flagHealth == 1)
            {
                enemyHealthBar.fillAmount -= amountHPFillImage;
                flagHealth = 0;
            }

            if (enemyDamageEntry.fillAmount > (auxFillAmount - amountHPFillImage))
                enemyDamageEntry.fillAmount -= Time.deltaTime;

            if (enemyDamageEntry.fillAmount <= (auxFillAmount - amountHPFillImage))
            {
                enemyDamageEntry.fillAmount = (auxFillAmount - amountHPFillImage);
                auxFillAmount = enemyDamageEntry.fillAmount;
                enemyUIUpdate = false;
            }
        }

        void UpdateEnemyRangerHP()
        {
            if (enemyReference2 == null)
                return;

            amountHPFillImage = (amountHPLose * 1) / enemyReference2.enemyMaxHP;

            if (flagHealth == 1)
            {
                enemyHealthBar.fillAmount -= amountHPFillImage;
                flagHealth = 0;
            }

            if (enemyDamageEntry.fillAmount > (auxFillAmount - amountHPFillImage))
                enemyDamageEntry.fillAmount -= Time.deltaTime;

            if (enemyDamageEntry.fillAmount <= (auxFillAmount - amountHPFillImage))
            {
                enemyDamageEntry.fillAmount = (auxFillAmount - amountHPFillImage);
                auxFillAmount = enemyDamageEntry.fillAmount;
                enemyUIUpdate = false;
            }
        }
    }
}