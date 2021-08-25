﻿using UnityEngine;

namespace Proto1
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] public float speed;
        [SerializeField] Animator playerAnimator;
        [SerializeField] SpriteRenderer mySprite;
        PlayerAttack playerAttack;
        Rigidbody2D rig;
        RoomID actualRoom;
        [SerializeField] int playerOnRoom;

        bool canDodge;

        [SerializeField] public GameObject dodgeTrails;
        [SerializeField] public float dodgeRecovery;
        [SerializeField][Range(1,4)] public float recoverDodge;


        void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            playerAttack = GetComponent<PlayerAttack>();
        }

        void Start()
        {
            canDodge = true;
            playerOnRoom = 0;
        }

        void Update()
        {
            if (dodgeRecovery <= recoverDodge)
                dodgeRecovery += Time.deltaTime;

            if(dodgeRecovery > recoverDodge)
            {
                dodgeRecovery = recoverDodge;
            }

            if(dodgeRecovery == recoverDodge)
            {
                canDodge = true;
                dodgeTrails.gameObject.SetActive(false);
            }

            Movement();
        }

        public RoomID GetPlayerOnRoom()
        {
            return actualRoom;
        }

        void SetPlayerOnRoom(RoomID room)
        {
            if (room == null)
                return;

            actualRoom = room;
            playerOnRoom = room.roomId;
        }

        void DodgeInDirection(Vector2 direction)
        {
            if (!canDodge)
                return;

            if (Input.GetKeyDown(KeyCode.Space) && rig.velocity != Vector2.zero)
            {
                rig.AddForce(direction * 1.5f, ForceMode2D.Impulse);
                playerAnimator.SetTrigger("dodge");
                dodgeTrails.gameObject.SetActive(true);

                dodgeRecovery = 0;
                canDodge = false;
            }
        }

        void Movement()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            Vector2 position = new Vector2(x * Time.deltaTime * speed, y * Time.deltaTime * speed);

            rig.AddForce(position);

            DodgeInDirection(position);

            if (playerAttack.attackColdown > 0)
                rig.velocity = Vector3.zero;

            if (playerAnimator != null)
            {
                if (position != Vector2.zero)
                {
                    if (position.x < 0)
                        mySprite.flipX = true;
                    else
                        mySprite.flipX = false;


                    playerAnimator.SetFloat("speed", position.magnitude);
                }
                else
                {
                    playerAnimator.SetFloat("speed", 0);
                }
            }

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

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("RoomSpace"))
            {
                actualRoom = collision.GetComponent<RoomID>();
                SetPlayerOnRoom(actualRoom);
            }
        }
    }
}
