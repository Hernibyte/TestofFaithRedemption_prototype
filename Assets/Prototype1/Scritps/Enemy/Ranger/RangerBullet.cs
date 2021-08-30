using UnityEngine;

namespace Proto1
{
    public class RangerBullet : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;
        [SerializeField] LayerMask playerLayer;
        int damage;
        float knockBackForce;

        public void FixRotation(Vector2 fireDirection)
        {
            Vector2 directionShoot = fireDirection;
            float angle = Mathf.Atan2(directionShoot.y, directionShoot.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }

        public void SetBulletParams(int damageIn, float knockBack)
        {
            damage = damageIn;
            knockBackForce = knockBack;
        }

        bool Contains(LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(Contains(playerLayer, collision.gameObject.layer))
            {
                IHittable hit = collision.GetComponent<IHittable>();
                if(hit != null)
                {
                    hit.Hit(damage, knockBackForce, rb.position);
                    Destroy(gameObject);
                }
            }
        }
    }
}