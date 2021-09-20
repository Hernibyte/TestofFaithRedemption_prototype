using UnityEngine;

namespace Proto1
{
    public class Crow : MonoBehaviour
    {
        [SerializeField] LayerMask enemyLayer;
        [SerializeField] LayerMask roomLayer;
        [SerializeField] GameObject crowEffect;
        public Rigidbody2D rig;
        bool alive = true;

        [SerializeField] int damageCrow;
        float knockbackCrow;

        float timeToIncrease = 0.2f;
        float timerToPowerDamage;

        private void Update()
        {
            if(alive)
            {
                if (timerToPowerDamage < timeToIncrease)
                    timerToPowerDamage += Time.deltaTime;

                if(timerToPowerDamage > timeToIncrease)
                {
                    damageCrow += 4;
                    timerToPowerDamage = 0;
                }
            }
        }

        public bool Contains(LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        public void SetCrowStats(int damage,float knockback)
        {
            damageCrow = (int)(damage * 0.5f);
            knockbackCrow = 0;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Contains(enemyLayer, collision.gameObject.layer))
            {
                IHittable hitObj = collision.GetComponent<IHittable>();
                if (hitObj != null)
                {
                    hitObj.Hit(damageCrow, knockbackCrow, rig.position);

                    GameObject go = Instantiate(crowEffect, rig.position, Quaternion.identity);
                    if (go != null)
                    {
                        Animator crowEff = go.GetComponent<Animator>();
                        if (crowEff != null)
                        {
                            crowEff.Play("CrowAttack");
                        }
                    }
                    alive = false;
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (Contains(roomLayer, collision.gameObject.layer))
            {
                Destroy(gameObject);
            }
        }
    }
}