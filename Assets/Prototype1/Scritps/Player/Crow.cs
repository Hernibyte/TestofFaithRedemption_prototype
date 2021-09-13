using UnityEngine;

namespace Proto1
{
    public class Crow : MonoBehaviour
    {
        [SerializeField] LayerMask enemyLayer;
        public Rigidbody2D rig;
        bool alive = true;

        int damageCrow;
        float knockbackCrow;

        private void Update()
        {
            if(alive)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(rig.position, 0.5f, enemyLayer);
                foreach(Collider2D hit in hits)
                {
                    IHittable hitObj = hit.GetComponent<IHittable>();
                    if(hitObj != null)
                    {
                        hitObj.Hit(damageCrow, knockbackCrow, rig.position);
                        alive = false;
                        Destroy(gameObject);
                    }
                }
            }
        }
        public void SetCrowStats(int damage,float knockback)
        {
            damageCrow = damage;
            knockbackCrow = 0;
        }
    }
}