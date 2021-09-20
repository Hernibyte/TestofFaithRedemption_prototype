using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto1
{
    public class MeleeAttack : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;
        [SerializeField] PlayerMovement followPlayer;
        [SerializeField] Vector2 mousePosition;
    
        void Update()
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.position = followPlayer.rig.position;
        }

        void FixedUpdate()
        {
            Vector2 direction = mousePosition - rb.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
    }
}