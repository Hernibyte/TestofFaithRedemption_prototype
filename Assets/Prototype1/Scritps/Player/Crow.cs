using UnityEngine;

namespace Proto1
{
    public class Crow : MonoBehaviour
    {
        [SerializeField] LayerMask enemyLayer;
        public Rigidbody2D rig;
        bool alive = true;

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
                        hitObj.Hit(10, 1, rig.position);
                        alive = false;
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}