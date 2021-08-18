using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto1
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float speed;
        PlayerAttack playerAttack;
        Rigidbody2D rig;

        void Awake() 
        {
            rig = GetComponent<Rigidbody2D>();
            playerAttack = GetComponent<PlayerAttack>();
        }

        void Start()
        {

        }

        void Update()
        {
            Movement();
        }

        void Movement()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            Vector2 position = new Vector2(x * Time.deltaTime * speed, y * Time.deltaTime * speed);

            rig.AddForce(position);

            if (x > 0)
            {
                playerAttack.horizontalAttack = 0.4f;
                playerAttack.verticalAttack = 0;
            }
            else if (x < 0)
            {
                playerAttack.horizontalAttack = -0.4f;
                playerAttack.verticalAttack = 0;
            }

            if (y > 0)
            {
                playerAttack.verticalAttack = 0.4f;
                playerAttack.horizontalAttack = 0;
            }
            else if (y < 0)
            {
                playerAttack.verticalAttack = -0.4f;
                playerAttack.horizontalAttack = 0;
            }
        }
    }
}
