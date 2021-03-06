using UnityEngine;

namespace Proto1
{
    public class RangeAttack : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;
        [SerializeField] PlayerMovement followPlayer;
        [SerializeField] PlayerAttack playerStats;
        [SerializeField] Vector2 mousePosition;
        [SerializeField] Camera cam;

        [SerializeField] Transform firePoint;
        [SerializeField] Crow crowMagic;
        float crowForce = 5;

        private void Update()
        {
            mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            rb.position = followPlayer.rig.position;
        }
        void FixedUpdate()
        {
            Vector2 direction = mousePosition - rb.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
        public void Shoot()
        {
            Crow crowCreated = Instantiate(crowMagic, firePoint.position, firePoint.rotation);
            crowCreated.SetCrowStats(playerStats.playerDamage, playerStats.knockBackMelee);
            crowCreated.rig.AddForce(firePoint.up * crowForce, ForceMode2D.Impulse);
        }
    }
}