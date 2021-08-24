using UnityEngine;
using UnityEngine.UI;

namespace Proto1
{
    public class UI_Enemy : MonoBehaviour
    {
        [SerializeField] Enemy enemyReference;
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
        }
    
        void Update()
        {
            if(enemyUIUpdate)
            {
                 UpdateEnemyHP();   
            }
        }

        private void OnDisable()
        {
            enemyReference.updateUIData -= AskForUpdate;
        }

        void AskForUpdate(int amountDamage)
        {
            enemyUIUpdate = true;
            amountHPLose = amountDamage;
            flagHealth = 1;
        }

        void UpdateEnemyHP()
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
    }
}