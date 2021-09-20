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
        [SerializeField] MeleeEnemy enemyReference;
        [SerializeField] RangeEnemy enemyReference2;
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

            enemyReference.updateUIData += AskForUpdate;
            enemyReference2.updateUIData += AskForUpdate;
        }
    
        void Update()
        {
            if(enemyUIUpdate)
            {
                switch (enemyType)
                {
                    case TYPENEMY.Ranger:  UpdateEnemyRangerHP();
                        break;
                    case TYPENEMY.Warrior: UpdateEnemyWarriorHP();
                        break;
                }
            }
        }

        private void OnDisable()
        {
            enemyReference.updateUIData -= AskForUpdate;
            enemyReference2.updateUIData -= AskForUpdate;
        }

        void AskForUpdate(int amountDamage)
        {
            enemyUIUpdate = true;
            amountHPLose = amountDamage;
            flagHealth = 1;
        }

        void UpdateEnemyWarriorHP()
        {
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

        void UpdateEnemyRangerHP()
        {
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