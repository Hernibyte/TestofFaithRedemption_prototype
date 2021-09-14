using UnityEngine;

namespace Proto1
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] public float speed;
        [SerializeField] Animator playerAnimator;
        [SerializeField] SpriteRenderer mySprite;
        [SerializeField] Transform attackPoint;
        PlayerAttack playerAttack;
        public Rigidbody2D rig;
        [HideInInspector] public RoomID actualRoom;
        [SerializeField] public int playerOnRoom;

        bool canDodge;

        [SerializeField] public float decreaseSpeedAttack;
        [SerializeField] public GameObject dodgeTrails;
        [SerializeField] public float dodgeRecovery;
        [SerializeField][Range(1,4)] public float recoverDodge;

        void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            playerAttack = GetComponentInChildren<PlayerAttack>();
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

            if(dodgeRecovery >= recoverDodge / 2)
            {
                dodgeTrails.gameObject.SetActive(false);
            }
            if(dodgeRecovery == recoverDodge)
            {
                canDodge = true;
            }

        }

        private void FixedUpdate()
        {
            Movement();
        }

        public RoomID GetPlayerOnRoom()
        {
            return actualRoom;
        }

        public void SetPlayerOnRoom(RoomID room)
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

            if (Input.GetKey(KeyCode.Space) && rig.velocity != Vector2.zero)
            {
                rig.AddForce(direction / 2, ForceMode2D.Impulse);
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

            Vector2 position = new Vector2(x * speed, y * speed);

            rig.AddForce(position);

            DodgeInDirection(position);

            if (playerAttack.attackColdownMelee > playerAttack.attackSpeedMelee - decreaseSpeedAttack)
                rig.velocity = Vector3.zero;

            if (playerAnimator != null)
            {
                if(attackPoint.position.x > transform.position.x)
                    mySprite.flipX = false;
                else
                    mySprite.flipX = true;

                if (position != Vector2.zero)
                {
                    playerAnimator.SetFloat("speed", position.magnitude);
                }
                else
                {
                    playerAnimator.SetFloat("speed", 0);
                }
            }
        }
    }
}
