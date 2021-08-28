using UnityEngine;

namespace Proto1
{
    public class RangeAttack : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;
        [SerializeField] PlayerMovement followPlayer;
        [SerializeField] Vector2 mousePosition;
        [SerializeField] Camera cam;

        [SerializeField] Transform firePoint;
        [SerializeField] Crow crowMagic;
        float crowForce = 5;

        public void Shoot()
        {
            Crow crowCreated = Instantiate(crowMagic, firePoint.position, firePoint.rotation);
            crowCreated.rig.AddForce(firePoint.up * crowForce, ForceMode2D.Impulse);
        }
        private void Update()
        {
            mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Mouse Pos: " + mousePosition.ToString());
            rb.position = followPlayer.rig.position;
        }
        void FixedUpdate()
        {
            Vector2 direction = mousePosition - rb.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }
}