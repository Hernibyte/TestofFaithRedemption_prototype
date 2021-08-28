using UnityEngine;

namespace Proto1
{
    public class Crow : MonoBehaviour
    {
        [SerializeField] LayerMask enemyLayer;
        public Rigidbody2D rig;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == enemyLayer)
            {
                Destroy(other.gameObject);
            }

            //Destroy(gameObject);
        }
    }
}